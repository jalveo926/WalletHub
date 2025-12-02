const API_URL = 'https://localhost:7258/api';

let transacciones = [];

// ================== RESUMEN (CalculosController) ==================
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
  console.log('Resumen recibido:', resultado);

  if (!resultado.exito || !resultado.datos) {
    console.error('No hay datos de resumen para el periodo.');
    return;
  }

  const datos = resultado.datos;
  console.log('Datos dentro de resumen:', datos);

  const totalIngresos = datos.totalIngresos ?? 0;
  const totalGastos = datos.totalGastos ?? 0;
  const saldoCalculado =
    datos.saldo ??
    datos.diferencia ??
    (totalIngresos - totalGastos);

  document.getElementById('totalIngresos').textContent =
    `$${Number(totalIngresos).toFixed(2)}`;
  document.getElementById('totalGastos').textContent =
    `$${Number(totalGastos).toFixed(2)}`;
  document.getElementById('saldo').textContent =
    `$${Number(saldoCalculado).toFixed(2)}`;
}

// ================== TRANSACCIONES PARA TABLAS ==================
async function cargarTransaccionesPeriodo(periodo) {
  const token = localStorage.getItem('token');
  if (!token) {
    window.location.href = 'autenticacion.html';
    return;
  }

  const resp = await fetch(`${API_URL}/Transaccion/MisTransacciones`, {
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
    console.error('Error al obtener transacciones');
    return;
  }

  const resultado = await resp.json(); // { mensaje, transacciones }
  transacciones = resultado.transacciones || [];
  console.log('Transacciones cargadas:', transacciones);

  aplicarFiltroTablas(periodo);
}

function aplicarFiltroTablas(periodo) {
  const hoy = new Date();
  let inicio = new Date();

  if (periodo === 'semana') {
    inicio.setDate(hoy.getDate() - 7);
  } else if (periodo === 'mes') {
    inicio.setMonth(hoy.getMonth() - 1);
  } else if (periodo === 'anio') {
    inicio.setFullYear(hoy.getFullYear() - 1);
  }

  const filtradas = transacciones.filter(t => {
    const f = new Date(t.fechaTransac);
    return f >= inicio && f <= hoy;
  });

  const ingresos = filtradas.filter(t => t.tipoCategoria === 'Ingreso');
  const gastos = filtradas.filter(t => t.tipoCategoria === 'Gasto');

  renderTablas(ingresos, gastos);
}

function renderTablas(ingresos, gastos) {
  const tbodyIngresos = document.querySelector('#tablaIngresos tbody');
  const tbodyGastos = document.querySelector('#tablaGastos tbody');

  tbodyIngresos.innerHTML = '';
  tbodyGastos.innerHTML = '';

  ingresos.forEach(t => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td>${t.fechaTransac.split('T')[0]}</td>
      <td>${t.nombreCateg}</td>
      <td>${t.descripcionTransac}</td>
      <td>$${Number(t.montoTransac).toFixed(2)}</td>
    `;
    tbodyIngresos.appendChild(tr);
  });

  gastos.forEach(t => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td>${t.fechaTransac.split('T')[0]}</td>
      <td>${t.nombreCateg}</td>
      <td>${t.descripcionTransac}</td>
      <td>$${Number(t.montoTransac).toFixed(2)}</td>
    `;
    tbodyGastos.appendChild(tr);
  });
}

// ================== INICIALIZACIÓN ==================
document.addEventListener('DOMContentLoaded', () => {
  const periodoSelect = document.getElementById('periodoSelect');

  const periodoInicial = periodoSelect.value; // 'semana' por defecto
  cargarResumen(periodoInicial);
  cargarTransaccionesPeriodo(periodoInicial);

  periodoSelect.addEventListener('change', () => {
    const periodo = periodoSelect.value;
    cargarResumen(periodo);
    aplicarFiltroTablas(periodo); // usa las transacciones ya cargadas
  });
});
