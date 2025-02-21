// App.tsx
import React, { useState, useEffect } from 'react';
import ReservacionFormulario from './components/ReservacionFormulario';
import ListaReservacion from './components/ListaReservacion';

// Definición de interfaces para tipar las reservas y el servicio asociado
interface Servicio {
  id: number;
  nombre: string;
  precio: number;
  duracion: string; // Puede representarse como string (ej. "00:30:00")
}

export interface Reservation {
  id: number;
  cliente: string;
  fecha: string; // Se espera una fecha en formato ISO (YYYY-MM-DD)
  turno: number;
  servicio: Servicio;
}

const App: React.FC = () => {
  const [reservations, setReservations] = useState<Reservation[]>([]);

  // Función para obtener las reservas desde la API
  const fetchReservations = async () => {
    try {
      const response = await fetch('/api/reservas');
      if (!response.ok) {
        throw new Error('Error al obtener reservas');
      }
      const data: Reservation[] = await response.json();
      setReservations(data);
    } catch (error) {
      console.error('Error fetching reservations:', error);
    }
  };

  useEffect(() => {
    fetchReservations();
  }, []);

  return (
    <div className="container">
      <h1>Gestión de Reservas - Peluquería</h1>
      {/* Se pasa la función para refrescar la lista cuando se crea una nueva reserva */}
      <ReservacionFormulario onReservationCreated={fetchReservations} />
      <ListaReservacion reservations={reservations} />
    </div>
  );
};

export default App;
