import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

interface Servicio {
  id: number;
  nombre: string;
  precio: number;
  duracion: {
    ticks: number;
  };
}

interface ReservaRequest {
  cliente: string;
  servicioId: number;
  fecha: string;
  horario: number;
}

interface ReservaFormularioProps {
  onReservaCreada: () => void;
}

const ReservaFormulario: React.FC<ReservaFormularioProps> = ({ onReservaCreada }) => {
  const [servicios, setServicios] = useState<Servicio[]>([]);
  const [cliente, setCliente] = useState<string>('');
  const [servicioId, setServicioId] = useState<number | null>(null);
  const [fecha, setFecha] = useState<string>('');
  const [horario, setHorario] = useState<string>('');
  const [mensaje, setMensaje] = useState<string>('');

  useEffect(() => {
    const fetchServicios = async () => {
      try {
        const res = await fetch('http://localhost:5029/api/servicios');
        if (!res.ok) {
          throw new Error(`Error: ${res.status} ${res.statusText}`);
        }
        const data = await res.json();
        setServicios(data);
      } catch (error) {
        console.error('Error fetching servicios:', error);
        setMensaje('Error al cargar los servicios.');
      }
    };

    fetchServicios();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!cliente || servicioId === null || !fecha || !horario) {
      setMensaje('Por favor completa todos los campos.');
      return;
    }

    const horarioComoInt = parseInt(horario, 10);

    const reserva: ReservaRequest = {
      cliente,
      servicioId: servicioId,
      fecha: new Date(fecha).toISOString(),
      horario: horarioComoInt,
    };

    try {
      const res = await fetch('http://localhost:5029/api/Reservas', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(reserva),
      });

      if (!res.ok) {
        const errorData = await res.json();
        setMensaje(errorData.mensaje || 'Error al crear la reserva.');
        return;
      }

      const data = await res.json();
      setMensaje(data.mensaje);
      setCliente('');
      setServicioId(null);
      setFecha('');
      setHorario('');
      onReservaCreada();
    } catch (error) {
      console.error('Error al crear la reserva:', error);
      setMensaje('Error al crear la reserva.');
    }
  };

  return (
    <div className="container mt-5">
      <h2>Crear Reserva</h2>
      {mensaje && <div className="alert alert-danger">{mensaje}</div>}
      <form onSubmit={handleSubmit} className="needs-validation">
        <div className="mb-3">
          <label className="form-label">Nombre del Cliente:</label>
          <input
            type="text"
            className="form-control"
            placeholder="Nombre del cliente"
            value={cliente}
            onChange={(e) => setCliente(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Servicio:</label>
          <select
            className="form-select"
            value={servicioId !== null ? servicioId : ''}
            onChange={(e) => setServicioId(e.target.value ? Number(e.target.value) : null)}
          >
            <option value="">Seleccione un servicio</option>
            {servicios.map((servicio) => (
              <option key={servicio.id} value={servicio.id}>
                {servicio.nombre} - ${servicio.precio} ({servicio.duracion.ticks})
              </option>
            ))}
          </select>
        </div>
        <div className="mb-3">
          <label className="form-label">Fecha:</label>
          <input
            type="date"
            className="form-control"
            value={fecha}
            onChange={(e) => setFecha(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Horario:</label>
          <input
            type="time"
            className="form-control"
            value={horario}
            onChange={(e) => setHorario(e.target.value)}
          />
        </div>
        <button type="submit" className="btn btn-primary">Reservar</button>
      </form>
    </div>
  );
};

export default ReservaFormulario;
