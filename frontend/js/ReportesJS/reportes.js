const API_URL = 'https://localhost:7258/api'; /* URL base de la API */

let transacciones = []; /* Almacena todas las transacciones cargadas */

// ================== RESUMEN (CalculosController) ==================
async function cargarResumen(periodo) {
  const token = localStorage.getItem('token'); /* Obtiene el token */
  if (!token) { /* Si no hay token, redirige a login */
    window.location.href = '../pages/autenticacion.html';
    return;
  }

  const hoy = new Date(); /* Fecha actual */
  let inicio = new Date(); /* Fecha de inicio según periodo */

  if (periodo === 'semana') { /* Últimos 7 días */
    inicio.setDate(hoy.getDate() - 7);
  } else if (periodo === 'mes') { /* Último mes */
    inicio.setMonth(hoy.getMonth() - 1);
  } else if (periodo === 'anio') { /* Último año */
    inicio.setFullYear(hoy.getFullYear() - 1);
  }

  const inicioStr = inicio.toISOString().split('T')[0]; // yyyy-MM-dd
  const finStr = hoy.toISOString().split('T')[0]; 

  const resp = await fetch(`${API_URL}/Calculos/ObtenerResumen?inicio=${inicioStr}&fin=${finStr}`, { /* Llamada al endpoint */
    method: 'GET', 
    headers: { /* Headers con token */
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    }
  });

  if (resp.status === 401) { /* Si no autorizado, redirige a login */
    mostrarPopup('Tu sesión expiró. Inicia sesión de nuevo.');
    window.location.href = '../pages/autenticacion.html';
    return;
  }

  if (!resp.ok) { /* Si hay error en la respuesta */
    console.error('Error al obtener resumen');
    return;
  }

  const resultado = await resp.json(); // { exito, mensaje, datos }
  console.log('Resumen recibido:', resultado);

  if (!resultado.exito || !resultado.datos) { /* Si no hay datos */
    console.error('No hay datos de resumen para el periodo.');
    return;
  }

    const datos = resultado.datos; // { totalIngresos, totalGastos, diferencia }

    const totalIngresos = datos.totalIngresos ?? 0; /* Valores por defecto a 0 */
    const totalGastos = datos.totalGastos ?? 0; /* Valores por defecto a 0 */
    const diferencia = datos.diferencia ?? (totalIngresos - totalGastos); /* Calcula diferencia si no viene */

    /* Actualiza el DOM con los valores formateados */
    document.getElementById('totalIngresos').textContent = `$${Number(totalIngresos).toFixed(2)}`; /* Formatea a 2 decimales */
    document.getElementById('totalGastos').textContent = `$${Number(totalGastos).toFixed(2)}`;
    document.getElementById('saldo').textContent = `$${Number(diferencia).toFixed(2)}`;

}

// ================== TRANSACCIONES PARA TABLAS ==================
async function cargarTransaccionesPeriodo(periodo) { 
  const token = localStorage.getItem('token');
  if (!token) { /* Si no hay token, redirige a login */
    window.location.href = '../pages/autenticacion.html';
    return;
  }

  const resp = await fetch(`${API_URL}/Transaccion/MisTransacciones`, { /* Llamada al endpoint */
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    }
  });

  if (resp.status === 401) { /* Si no autorizado, redirige a login */
    mostrarPopup('Tu sesión expiró. Inicia sesión de nuevo.');
    window.location.href = '../pages/autenticacion.html';
    return;
  }

  if (!resp.ok) { /* Si hay error en la respuesta */
    console.error('Error al obtener transacciones');
    return;
  }

  const resultado = await resp.json(); // { mensaje, transacciones }
  transacciones = resultado.transacciones || []; /* Guarda todas las transacciones */
  console.log('Transacciones cargadas:', transacciones);

  aplicarFiltroTablas(periodo); /* Filtra y renderiza las tablas */
}

function aplicarFiltroTablas(periodo) { /* Filtra transacciones según periodo */
  const hoy = new Date(); /* Fecha actual */
  let inicio = new Date(); /* Fecha de inicio según periodo */

  if (periodo === 'semana') {
    inicio.setDate(hoy.getDate() - 7);
  } else if (periodo === 'mes') {
    inicio.setMonth(hoy.getMonth() - 1);
  } else if (periodo === 'anio') {
    inicio.setFullYear(hoy.getFullYear() - 1);
  }

  const filtradas = transacciones.filter(t => { /* Filtra por fecha */
    const f = new Date(t.fechaTransac);
    return f >= inicio && f <= hoy;
  });

  /* Separa en ingresos y gastos */
  const ingresos = filtradas.filter(t => t.tipoCategoria === 'Ingreso');
  const gastos = filtradas.filter(t => t.tipoCategoria === 'Gasto');

  renderTablas(ingresos, gastos);
}

function renderTablas(ingresos, gastos) { /* Renderiza las tablas en el DOM */
  const tbodyIngresos = document.querySelector('#tablaIngresos tbody'); /* Selecciona los cuerpos de las tablas */
  const tbodyGastos = document.querySelector('#tablaGastos tbody'); /* Selecciona los cuerpos de las tablas */

  tbodyIngresos.innerHTML = ''; /* Limpia tablas */
  tbodyGastos.innerHTML = ''; 

  ingresos.forEach(t => { /* Agrega filas a la tabla de ingresos */
    const tr = document.createElement('tr'); /* Crea una fila */
    tr.innerHTML = ` 
      <td>${t.fechaTransac.split('T')[0]}</td>
      <td>${t.nombreCateg}</td>
      <td>${t.descripcionTransac}</td>
      <td>$${Number(t.montoTransac).toFixed(2)}</td>
    `; /* Llena la fila */

    tbodyIngresos.appendChild(tr); /* Agrega la información de una fila a la tabla que dejamos vacia */
  });

  gastos.forEach(t => { /* Agrega filas a la tabla de gastos */
    const tr = document.createElement('tr'); /* Crea una fila */
    tr.innerHTML = `
      <td>${t.fechaTransac.split('T')[0]}</td>
      <td>${t.nombreCateg}</td>
      <td>${t.descripcionTransac}</td>
      <td>$${Number(t.montoTransac).toFixed(2)}</td>
    `; /* Llena la fila */

    tbodyGastos.appendChild(tr); /* Agrega la información de una fila a la tabla que dejamos vacia */
  });
}

// ================== INICIALIZACIÓN ==================
document.addEventListener('DOMContentLoaded', () => { /* Al cargar el DOM */
  const periodoSelect = document.getElementById('periodoSelect'); /* Selector de periodo */

  const periodoInicial = periodoSelect.value; // 'semana' por defecto
  cargarResumen(periodoInicial); /* Carga resumen inicial */
  cargarTransaccionesPeriodo(periodoInicial); /* Carga transacciones iniciales */

  periodoSelect.addEventListener('change', () => { /* Al cambiar el periodo */
    const periodo = periodoSelect.value; /* Nuevo periodo */
    cargarResumen(periodo); /* Actualiza resumen */
    aplicarFiltroTablas(periodo); // usa las transacciones ya cargadas
  });
});
