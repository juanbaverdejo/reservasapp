import React, { useState, useEffect, ChangeEvent, FormEvent } from 'react';

// Interfaces para tipar el servicio y el objeto de reserva que se enviará al backend
interface Service {
  id: number;
  nombre: string;
  precio: number;
  duracion: string; // Por ejemplo: "00:30:00"
}

interface ReservaRequest {
  Cliente: string;
  ServicioId: number;
  Fecha: string; // Se espera formato YYYY-MM-DD
  Turno: number;
}

interface ReservationFormProps {
  onReservationCreated: () => void;
}

const ReservacionFormulario: React.FC<ReservationFormProps> = ({ onReservationCreated }) => {
  const [services, setServices] = useState<Service[]>([]);
  const [availableTurnos, setAvailableTurnos] = useState<number[]>([]);
  
  const [cliente, setCliente] = useState<string>('');
  const [servicioId, setServicioId] = useState<string>('');
  const [fecha, setFecha] = useState<string>('');
  const [turno, setTurno] = useState<string>('');
  const [message, setMessage] = useState<string>('');

  useEffect(() => {
    const fetchServices = async () => {
      try {
        const res = await fetch('/api/servicios');
        if (!res.ok) {
          throw new Error('Error al obtener servicios');
        }
        const data = await res.json();
        setServices(data);
      } catch (error) {
        console.error('Error fetching services:', error);
      }
    };

    fetchServices();
  }, []);

  const fetchAvailableTurnos = async (selectedDate: string) => {
    try {
      const res = await fetch(`/api/horarios/disponibles?fecha=${selectedDate}`);
      if (!res.ok) {
        throw new Error('Error al obtener turnos disponibles');
      }
      const data = await res.json();
      setAvailableTurnos(data);
    } catch (error) {
      console.error('Error fetching available turnos:', error);
    }
  };

  const handleFechaChange = (e: ChangeEvent<HTMLInputElement>) => {
    const selectedDate = e.target.value;
    setFecha(selectedDate);
    fetchAvailableTurnos(selectedDate);
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!cliente || !servicioId || !fecha || !turno) {
      setMessage('Por favor completa todos los campos.');
      return;
    }

    const reserva: ReservaRequest = {
      Cliente: cliente,
      ServicioId: parseInt(servicioId, 10),
      Fecha: fecha,
      Turno: parseInt(turno, 10)
    };

    try {
      const res = await fetch('/api/reservas', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(reserva)
      });
      
      if (res.ok) {
        const data = await res.json();
        setMessage('Reserva creada exitosamente.');
        // Reiniciar los campos del formulario
        setCliente('');
        setServicioId('');
        setFecha('');
        setTurno('');
        setAvailableTurnos([]);
        onReservationCreated();
      } else {
        const errorData = await res.json();
        setMessage(errorData.message || 'Error al crear la reserva.');
      }
    } catch (error) {
      console.error('Error al crear la reserva:', error);
      setMessage('Error al crear la reserva.');
    }
  };

  return (
    <div className="reservation-form">
      <h2>Crear Reserva</h2>
      {message && <p>{message}</p>}
      <form onSubmit={handleSubmit}>
        <div>
          <label>Nombre del Cliente:</label>
          <input 
            type="text" 
            value={cliente} 
            onChange={(e) => setCliente(e.target.value)} 
          />
        </div>
        <div>
          <label>Servicio:</label>
          <select value={servicioId} onChange={(e) => setServicioId(e.target.value)}>
            <option value="">Seleccione un servicio</option>
            {services.map((service) => (
              <option key={service.id} value={service.id}>
                {service.nombre}
              </option>
            ))}
          </select>
        </div>
        <div>
          <label>Fecha:</label>
          <input 
            type="date" 
            value={fecha} 
            onChange={handleFechaChange} 
          />
        </div>
        <div>
          <label>Turno:</label>
          <select value={turno} onChange={(e) => setTurno(e.target.value)}>
            <option value="">Seleccione un turno</option>
            {availableTurnos.map((t) => (
              <option key={t} value={t}>
                {t}:00 hs
              </option>
            ))}
          </select>
        </div>
        <button type="submit">Reservar</button>
      </form>
    </div>
  );
};

export default ReservacionFormulario;
