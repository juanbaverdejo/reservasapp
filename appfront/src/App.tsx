import React, { useState } from 'react';
import ReservaFormulario from './components/ReservacionFormulario';
import ListadoReservas from './components/ListaReservacion';

const App: React.FC = () => {
  const [reservasActualizadas, setReservasActualizadas] = useState<boolean>(false);

  const handleReservaCreada = () => {
    setReservasActualizadas(!reservasActualizadas);
  };

  return (
    <div className="bg-light min-vh-100 d-flex flex-column justify-content-center">
      <h1 className="titulo text-center mb-4">Gestión de Reservas - Peluquería</h1>

      <div className="container bg-white p-4 rounded shadow">
        <ReservaFormulario onReservaCreada={handleReservaCreada} />
      </div>

      <div className="container mt-4">
        <ListadoReservas key={reservasActualizadas ? 'actualizado' : 'inicial'} />
      </div>
    </div>
  );
};

export default App;
