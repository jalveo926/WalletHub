const API_URL = 'https://localhost:7258/api';

async function cargarResumen(periodo) {
  const token = localStorage.getItem('token');
  if (!token) {
    window.location.href = 'autenticacion.html';
    return;
  }

  const hoy = new Date();
  let inicio = new Date();

  if (periodo === 'semana') {
    inicio.setDate(hoy.getDate() - 7);
  } else if (periodo === 'mes') {
    inicio.setMonth(hoy.getMonth() - 1);
  } else if (periodo === 'anio') {
    inicio.setFullYear(hoy.getFullYear() - 1);
  }

  const inicioStr = inicio.toISOString().split('T')[0]; // yyyy-MM-dd
  const finStr = hoy.toISOString().split('T')[0];

  const resp = await fetch(`${API_URL}/Calculos/resumen?inicio=${inicioStr}&fin=${finStr}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    }
  });

  if (resp.status === 401) {
    alert('Tu sesión expiró. Inicia sesión de nuevo.');
    window.location.href = 'autenticacion.html';
    return;
  }

  if (!resp.ok) {
    console.error('Error al obtener resumen');
    return;
  }

  const resultado = await resp.json(); // { exito, mensaje, datos }
  const datos = resultado.datos; // CalculosDTO

  // Suponiendo que el DTO trae algo como: totalIngresos, totalGastos, saldo
  document.getElementById('totalIngresos').textContent = `$${datos.totalIngresos.toFixed(2)}`;
  document.getElementById('totalGastos').textContent = `$${datos.totalGastos.toFixed(2)}`;
  document.getElementById('saldo').textContent = `$${datos.saldo.toFixed(2)}`;
}

document.addEventListener('DOMContentLoaded', () => {
  const periodoSelect = document.getElementById('periodoSelect');

  // Cargar resumen por defecto (última semana)
  cargarResumen(periodoSelect.value);

  periodoSelect.addEventListener('change', () => {
    cargarResumen(periodoSelect.value);
  });
});

