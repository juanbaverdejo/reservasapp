import React, { useEffect, useState } from 'react';

interface Reserva {
  id: number;
  cliente: string;
  servicioId: number;
  fecha: string;
  horario: number;
}

const ListadoReservas: React.FC = () => {
  const [reservas, setReservas] = useState<Reserva[]>([]);
  const [mensaje, setMensaje] = useState<string>('');

  useEffect(() => {
    fetch('http://localhost:5029/api/reservas')
      .then((res) => {
        if (!res.ok) {
          throw new Error('Error al obtener las reservas');
        }
        return res.json();
      })
      .then((data) => setReservas(data))
      .catch((error) => {
        console.error('Error fetching reservas:', error);
        setMensaje('Error al cargar las reservas.');
      });
  }, []);

  return (
    <div className="container bg-white p-4 rounded shadow mt-4">
      <h2 className="titulo text-center mb-4">Listado de Reservas</h2>
      {mensaje && (
        <div className="alert alert-danger" role="alert">
          {mensaje}
        </div>
      )}
      <ul className="list-group">
        {reservas.map((reserva) => (
          <li key={reserva.id} className="list-group-item">
            <strong>Cliente:</strong> {reserva.cliente} <br />
            <strong>Fecha:</strong> {new Date(reserva.fecha).toLocaleDateString()} <br />
            <strong>Horario:</strong> {reserva.horario} <br />
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ListadoReservas;
