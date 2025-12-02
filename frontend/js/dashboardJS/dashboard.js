function obtenerRango(opcion) { //Rango de gráficas
    const hoy = new Date();
    const inicio = new Date();

    switch (opcion) {
        case "semana":
            inicio.setDate(hoy.getDate() - 7);
            break;
        case "mes":
            inicio.setMonth(hoy.getMonth() - 1);
            break;
        case "ano":
            inicio.setFullYear(hoy.getFullYear() - 1);
            break;
        default:
            inicio.setDate(hoy.getDate() - 7);
    }
    // Convertimos a formato YYYY-MM-DD para API
    const fechaFin = hoy.toISOString().split("T")[0];
    const fechaInicio = inicio.toISOString().split("T")[0];
    return { fechaInicio, fechaFin };
}

// ==================== FUNCIONES PARA LLAMAR CREAR GRAFICAS ====================

const API_URL = 'https://localhost:7258/api';
let chartIngresosVsGastos = null;
let chartGastosPorCategoria = null;
let chartIngresosPorCategoria = null;

async function cargarResumen(fechaInicio, fechaFin) {
    const token = localStorage.getItem("token");

    const url = `${API_URL}/Calculos/resumen?inicio=${fechaInicio}&fin=${fechaFin}`;

    try {
        const respuesta = await fetch(url, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!respuesta.ok) {
            switch (respuesta.status) {
                case 400:
                    alert("Solicitud inválida. Revisa los datos enviados.");
                    break;
                case 401:
                    alert("No autorizado. Tu sesión expiró, inicia sesión nuevamente.");
                    window.location.href = "/autenticacion.html";
                    break;
                case 403:
                    alert("Acceso denegado.");
                    break;
                case 404:
                    alert("No se encontraron datos para el rango seleccionado.");
                    break;
                case 500:
                    alert("Error en el servidor. Inténtalo más tarde.");
                    break;
                default:
                    alert("Ocurrió un error inesperado.");
            }
            return;
        }

    const json = await respuesta.json();

    // validar estructura esperada
        if (!json.exito || !json.datos) {
            alert(json.mensaje || "El servidor no devolvió datos válidos.");
            return;
        }

    const datos = json.datos;

    cargarSaldoActual(datos.diferencia);
    crearIngresosVsGastos(datos.totalIngresos, datos.totalGastos);
    crearGastosPorCategoria(datos.gastosPorCategoria);
    crearIngresosPorCategoria(datos.ingresosPorCategoria);

    } catch (error) {
        console.error("Error en fetch:", error);
        alert("Error de conexión. Verifica tu internet o el servidor.");
    }
}

function cargarSaldoActual(diferencia) {
    const saldoElemento = document.getElementById("saldo-actual");
    saldoElemento.textContent = `$${diferencia.toFixed(2)}`;
}

function crearIngresosVsGastos(totalIngresos, totalGastos) {
    const ctx = document.getElementById('ingresos-vs-gastos').getContext('2d');

    if (chartIngresosVsGastos) chartIngresosVsGastos.destroy(); // Destruir gráfico previo si existe, permite actualizar nuevas gráficas

    chartIngresosVsGastos = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ['Total de ingresos', 'Total de gastos'],
        datasets: [{
            data: [100, 200]
        }]
    },
    options: {
        indexAxis: 'y',
        responsive: true
    }
    });
}

function crearGastosPorCategoria(gastosPorCategoria) {
    const ctx = document.getElementById('gastos-por-categoria').getContext('2d');

    if (chartGastosPorCategoria) chartGastosPorCategoria.destroy();

    chartGastosPorCategoria = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: gastosPorCategoria.map(item => item.nombreCategoria),
            datasets: [{
                data: gastosPorCategoria.map(item => item.total)
            }]
        },
        options: {
            responsive: true
        }
    });
}

function crearIngresosPorCategoria(ingresosPorCategoria) {
    const ctx = document.getElementById('ingresos-por-categoria').getContext('2d');

    if (chartIngresosPorCategoria) chartIngresosPorCategoria.destroy(); // Destruir gráfico previo si existe, permite actualizar nuevas gráficas

    chartIngresosPorCategoria = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ingresosPorCategoria.map(item => item.nombreCategoria),
            datasets: [{
                data: ingresosPorCategoria.map(item => item.total)
            }]
        },
        options: {
            responsive: true
        }
    });
}

document.addEventListener("DOMContentLoaded", () => {
    const { fechaInicio, fechaFin } = obtenerRango("semana");
    cargarResumen(fechaInicio, fechaFin);

    const combo = document.getElementById("combobox-filtro-tiempo");

    combo.addEventListener("change", () => {
        const { fechaInicio, fechaFin } = obtenerRango(combo.value);
        cargarResumen(fechaInicio, fechaFin);
    });
})