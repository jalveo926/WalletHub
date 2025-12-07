const API_URL = 'https://localhost:7258/api'; // URL base de la API
let transacciones = []; // Almacena todas las transacciones cargadas

//CARGAR RESUMEN

async function cargarResumen(periodo) {
    const token = localStorage.getItem('token'); // Obtener token
    if (!token) {
        window.location.href = '../pages/autenticacion.html'; // Redirigir si no hay token
        return;
    }

    const hoy = new Date();
    let inicio = new Date();

    // Determinar fecha de inicio según periodo
    if (periodo === 'semana') inicio.setDate(hoy.getDate() - 7);
    else if (periodo === 'mes') inicio.setMonth(hoy.getMonth() - 1);
    else if (periodo === 'anio') inicio.setFullYear(hoy.getFullYear() - 1);
    else if (periodo === 'todo') inicio = new Date(2000, 0, 1); // Fecha muy antigua para "todo"

    const inicioStr = inicio.toISOString().split('T')[0]; // Formato YYYY-MM-DD
    const finStr = hoy.toISOString().split('T')[0];

    // Llamada a la API para obtener resumen
    const resp = await fetch(`${API_URL}/Calculos/ObtenerResumen?inicio=${inicioStr}&fin=${finStr}`, { // Parámetros en query string
        method: 'GET',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` }
    });

    if (resp.status === 401) { // Token expirado
        alert('Tu sesión expiró. Inicia sesión de nuevo.');
        window.location.href = '../pages/autenticacion.html'; // Redirigir 
        return;
    }

    if (!resp.ok) { console.error('Error al obtener resumen'); return; } // Manejo de error

    const resultado = await resp.json(); // Parsear JSON
    if (!resultado.exito || !resultado.datos) { console.error('No hay datos de resumen'); return; } // Validar datos

    // Mostrar totales en la UI
    const datos = resultado.datos;
    document.getElementById('totalIngresos').textContent = `$${Number(datos.totalIngresos ?? 0).toFixed(2)}`; // Manejo de null/undefined
    document.getElementById('totalGastos').textContent = `$${Number(datos.totalGastos ?? 0).toFixed(2)}`; 
    document.getElementById('saldo').textContent = `$${Number(datos.diferencia ?? (datos.totalIngresos - datos.totalGastos)).toFixed(2)}`; // Calcular diferencia si no viene en datos
}

//CARGAR TRANSACCIONES

async function cargarTransaccionesPeriodo(periodo) { // Cargar todas las transacciones y filtrar después
    const token = localStorage.getItem('token');
    if (!token) { window.location.href = '../pages/autenticacion.html'; return; }

    const resp = await fetch(`${API_URL}/Transaccion/MisTransacciones`, { // Obtener todas las transacciones
        method: 'GET',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` }
    });

    if (resp.status === 401) { alert('Tu sesión expiró'); window.location.href = '../pages/autenticacion.html'; return; } // Token expirado
    if (!resp.ok) { console.error('Error al obtener transacciones'); return; } // Manejo de error

    const resultado = await resp.json(); // Parsear JSON
    transacciones = resultado.transacciones || []; // Guardar transacciones
    aplicarFiltroTablas(periodo); // Filtrar y renderizar
}

//FILTRAR Y RENDERIZAR TABLAS

function aplicarFiltroTablas(periodo) { // Filtrar transacciones según periodo
    const hoy = new Date(); // Fecha actual
    let inicio = new Date(); // Fecha de inicio

    if (periodo === 'semana') inicio.setDate(hoy.getDate() - 7); // Última semana
    else if (periodo === 'mes') inicio.setMonth(hoy.getMonth() - 1); // Último mes
    else if (periodo === 'anio') inicio.setFullYear(hoy.getFullYear() - 1); // Último año
    else if (periodo === 'todo') inicio = new Date(2000, 0, 1); // Fecha muy antigua para "todo"

    const filtradas = transacciones.filter(t => { // Filtrar por fecha
        const f = new Date(t.fechaTransac); // Convertir a Date
        return f >= inicio && f <= hoy;
    });

    const ingresos = filtradas.filter(t => t.tipoCategoria === 'Ingreso'); // Separar ingresos y gastos
    const gastos = filtradas.filter(t => t.tipoCategoria === 'Gasto');

    renderTablas(ingresos, gastos); // Renderizar tablas
}

function renderTablas(ingresos, gastos) { // Renderizar tablas de ingresos y gastos
    const tbodyIngresos = document.querySelector('#tablaIngresos tbody'); // Seleccionar cuerpos de tabla
    const tbodyGastos = document.querySelector('#tablaGastos tbody');

    // Limpiar tablas
    tbodyIngresos.innerHTML = '';
    tbodyGastos.innerHTML = '';

    // Render ingresos
    ingresos.forEach(t => {
        const tr = document.createElement('tr'); // Crear fila
        tr.innerHTML = `
            <td>${t.fechaTransac.split('T')[0]}</td> 
            <td>${t.nombreCateg}</td>
            <td>${t.descripcionTransac}</td>
            <td>$${Number(t.montoTransac).toFixed(2)}</td>
        `;
        tbodyIngresos.appendChild(tr); // Agregar fila a la tabla
    });

    // Render gastos
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

//GENERAR PDF

async function generarPDF(periodo) { // Generar y descargar PDF del reporte
    const token = localStorage.getItem('token');
    if (!token) { window.location.href = '../pages/autenticacion.html'; return; }

    const hoy = new Date(); // Fecha actual
    let inicio = new Date(); // Fecha de inicio
    if (periodo === 'semana') inicio.setDate(hoy.getDate() - 7); // Última semana
    else if (periodo === 'mes') inicio.setMonth(hoy.getMonth() - 1);
    else if (periodo === 'anio') inicio.setFullYear(hoy.getFullYear() - 1);
    else if (periodo === 'todo') inicio = new Date(2000, 0, 1);

    const mapPeriodo = { semana: "semana", mes: "mes", anio: "año", todo: "todo" }; // Mapeo para la API
    let dto = { tipoPeriodo: mapPeriodo[periodo] }; // DTO para la API
    if (periodo === "todo") { dto.inicio = inicio.toISOString(); dto.fin = hoy.toISOString(); } // Fechas para "todo"

    const resp = await fetch(`${API_URL}/Reporte/DescargarPdfPeriodo`, { // Llamada a la API
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify(dto)
    });

    if (!resp.ok) { console.error(await resp.text()); alert("Error generando PDF"); return; } // Manejo de error

    // Descargar PDF
    const blob = await resp.blob(); // Obtener blob del PDF
    const url = window.URL.createObjectURL(blob); // Crear URL temporal
    const a = document.createElement('a'); // Crear enlace de descarga
    a.href = url; // Asignar URL
    a.download = `Reporte_${periodo}.pdf`; // Nombre del archivo
    a.click(); // Iniciar descarga
    window.URL.revokeObjectURL(url); // Liberar URL
}

//INICIALIZACIÓN

document.addEventListener('DOMContentLoaded', () => { // Al cargar la página
    const periodoSelect = document.getElementById('periodoSelect'); // Selector de periodo
    const periodoInicial = periodoSelect.value; // Periodo inicial seleccionado

    cargarResumen(periodoInicial); // Cargar resumen inicial
    cargarTransaccionesPeriodo(periodoInicial); // Cargar transacciones iniciales

    // Cambiar periodo
    periodoSelect.addEventListener('change', () => { // Al cambiar el periodo
        const periodo = periodoSelect.value; // Nuevo periodo seleccionado
        cargarResumen(periodo); // Recargar resumen
        aplicarFiltroTablas(periodo); // Filtrar y renderizar tablas
    });

    // Botón exportar PDF
    const btnExportarPdf = document.getElementById('btnExportarPdf'); 
    if (btnExportarPdf) btnExportarPdf.addEventListener('click', () => generarPDF(periodoSelect.value)); // Generar PDF al hacer clic
});
