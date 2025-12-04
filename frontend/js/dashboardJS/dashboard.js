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

// ==================== FUNCIONES PARA CREAR GRAFICAS ====================

const API_URL = 'https://localhost:7258/api';
const token = localStorage.getItem("token");
let chartIngresosVsGastos = null;
let chartGastosPorCategoria = null;
let chartIngresosPorCategoria = null;

// Configuración global de Chart.js
Chart.defaults.font.family = "Nunito, sans-serif";
Chart.defaults.font.size = 16;
Chart.defaults.font.weight = "bold";
Chart.defaults.color = "#0A1F44";

async function cargarResumen(fechaInicio, fechaFin) {

    const url = `${API_URL}/Calculos/ObtenerResumen?inicio=${fechaInicio}&fin=${fechaFin}`;

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
                    localStorage.removeItem("token");
                    window.location.href = "../pages/autenticacion.html";
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

    crearIngresosVsGastos(datos.totalIngresos, datos.totalGastos);
    crearGastosPorCategoria(datos.gastosPorCategoria);
    crearIngresosPorCategoria(datos.ingresosPorCategoria);

    } catch (error) {
        console.error("Error en fetch:", error);
        alert("Error de conexión. Verifica tu internet o el servidor.");
    }
}

async function cargarSaldoActual() {
    const saldoElemento = document.getElementById("saldo-actual");

    const url = `${API_URL}/Calculos/ObtenerTotalesGenerales`;

    try {
        const response = await fetch(url, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });
        const data = await response.json();

        if (!data || !data.datos) {
            saldoElemento.textContent = "$0.00";
            return;
        }

        const {totalIngresos, totalGastos} = data.datos;
        const saldoActual = totalIngresos - totalGastos;
        if (saldoActual < 0) {
            saldoElemento.style.color = "#c0392b";
        } else {
            saldoElemento.style.color = "#2e7d32";
        }
        saldoElemento.textContent = `$${saldoActual.toFixed(2)}`;
    } catch (error) {
        console.error("Error al cargar saldo actual:", error);
        saldoElemento.textContent = "Error";
    }
}

function crearIngresosVsGastos(totalIngresos, totalGastos) {
    const ctx = document.getElementById('ingresos-vs-gastos').getContext('2d');

    if (chartIngresosVsGastos) chartIngresosVsGastos.destroy(); // Destruir gráfico previo si existe, permite actualizar nuevas gráficas

    chartIngresosVsGastos = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ['INGRESOS', 'GASTOS'],
        datasets: [{
            data: [totalIngresos, totalGastos],
            backgroundColor: ['#E8F8F2', '#FDEDEC'],
            borderColor: ['#2ecc71', '#e74c3c'],
            borderWidth: 2
        }]
    },
    options: {
        indexAxis: 'y', // Lo convierte en un gráfico de barras horizontal
        responsive: true,
        maintainAspectRatio: false, // Permite que el gráfico use todo el espacio del contenedor
        plugins: {
            title: {
                display: true,
                text: 'INGRESOS VS GASTOS',
                font: {
                    size: 20
                }
            },
            legend: {
                display: false
            },
            datalabels: {
                color: ["#2e7d32", "#c0392b"],
                anchor: "center",
                align: "center",
                font: {
                    size: 18,
                    weight: "bold"
                },
                formatter: (value) => {
                    return `$${value.toFixed(2)}`;
                }
            }
        },
        scales: {
            x: {
                grid: {
                    display: false
                }
            },
            y: {
                grid: {
                    display: false
                }
            }
        },
        animation: {
            duration: 900,
            easing: "easeOutQuart"
        },
        hover: {
            mode: "nearest",
            intersect: true
        }
    },
    plugins: [ChartDataLabels]
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
                data: gastosPorCategoria.map(item => item.total),
                backgroundColor: ['#FDEDEC', '#FADBD8', '#F5B7B1', '#F1948A', '#EC7063', '#E74C3C'],
                borderColor: ['#e74c3c'],
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: 'GASTOS POR CATEGORÍA',
                    font: {
                        size: 20
                    }
                },
                legend: {
                    display: true,
                    position: 'bottom',
                    labels: {
                        font: {
                            size: 14 
                        }
                    }
                },
                animation: {
                    duration: 900,
                    easing: "easeOutQuart"
                },
                hover: {
                    mode: "nearest",
                    intersect: true
                }
            }
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
                data: ingresosPorCategoria.map(item => item.total),
                backgroundColor: ['#E8F8F2', '#D1F2EB', '#A3E4D7', '#76D7C4', '#48C9B0', '#1ABC9C'],
                borderColor: ['#2ecc71'],
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: 'INGRESOS POR CATEGORÍA',
                    font: {
                        size: 20
                    }
                },
                legend: {
                    display: true,
                    position: 'bottom',
                    labels: {
                        font: {
                            size: 14 
                        }
                    }
                },
                animation: {
                    duration: 900,
                    easing: "easeOutQuart"
                },
                hover: {
                    mode: "nearest",
                    intersect: true
                }
            }
        }
    });
}

document.addEventListener("DOMContentLoaded", () => {
    const { fechaInicio, fechaFin } = obtenerRango("semana");
    cargarResumen(fechaInicio, fechaFin);
    cargarSaldoActual();

    const combo = document.getElementById("combobox-filtro-tiempo");

    combo.addEventListener("change", () => {
        const { fechaInicio, fechaFin } = obtenerRango(combo.value);
        cargarResumen(fechaInicio, fechaFin);
    });
})