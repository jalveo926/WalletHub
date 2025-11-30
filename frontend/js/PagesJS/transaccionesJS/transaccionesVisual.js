

// Variable global para almacenar las transacciones cargadas desde la API
let data = {
    mensaje: "",
    transacciones: []
}; 


// ------------------ CARGAR CATEGORÍAS ------------------
function cargarCategorias() {
    const filtroCategoria = document.getElementById("filtroCategoria");

    const categoriasUnicas = [...new Set(data.transacciones.map(t => t.nombreCateg))];

    categoriasUnicas.forEach(cat => {
        const option = document.createElement("option");
        option.value = cat;
        option.textContent = cat;
        filtroCategoria.appendChild(option);
    });
}


// ------------------ MOSTRAR TRANSACCIONES ------------------
function mostrarTransacciones(lista) {
    const contenedor = document.getElementById("lista-transacciones");
    contenedor.innerHTML = "";

    if (lista.length === 0) {
        contenedor.innerHTML = "<p>No hay transacciones para mostrar.</p>";
        return;
    }

   lista.forEach(t => {
    const tipoClase = t.tipoCategoria === "Ingreso" ? "ingreso" : "gasto";

    contenedor.innerHTML += `
        <div class="item ${tipoClase}">
            <div class="item-encabezado">
                <h3>${t.nombreCateg}</h3>
                <button class="btn-opciones"></button>
            </div>

            <p>${t.descripcionTransac}</p>
            <span>Monto: $${t.montoTransac}</span><br>
            <span>Fecha: ${new Date(t.fechaTransac).toLocaleDateString()}</span> 
        </div>
    `;
  });

}




// ------------------ APLICAR FILTROS ------------------
function aplicarFiltros() {
    const tipo = document.getElementById("filtroTipo").value;
    const categoria = document.getElementById("filtroCategoria").value;
    const montoMaximo = Number(document.getElementById("filtroMontoMax").value);

    const fechaDesde = document.getElementById("filtroFechaDesde").value;
    const fechaHasta = document.getElementById("filtroFechaHasta").value;



    let filtradas = data.transacciones;
    // Filtrar por monto máximo usando slider
    filtradas = filtradas.filter(t => t.montoTransac <= montoMaximo);


    // Validar rango de fechas
    if (fechaDesde !== "" && fechaHasta !== "") {
    if (new Date(fechaHasta) < new Date(fechaDesde)) {
        alert("La fecha 'Hasta' no puede ser menor que 'Desde'.");
        return;
    }
}

    // Filtrar por tipo
    if (tipo !== "Todos") {
        filtradas = filtradas.filter(t => t.tipoCategoria === tipo);
    }

    // Filtrar por categoría
    if (categoria !== "") {
        filtradas = filtradas.filter(t => t.nombreCateg === categoria);
    }

    // Filtrar por fecha DESDE
    if (fechaDesde !== "") {
        const desde = new Date(fechaDesde);
        filtradas = filtradas.filter(t => new Date(t.fechaTransac) >= desde);
    }

    // Filtrar por fecha HASTA
    if (fechaHasta !== "") {
        const hasta = new Date(fechaHasta);
        filtradas = filtradas.filter(t => new Date(t.fechaTransac) <= hasta);
    }

    mostrarTransacciones(filtradas);
}

function resetearFiltros() {
    
    // Resetear select de tipo
    document.getElementById("filtroTipo").value = "Todos";

    // Resetear select de categoría
    document.getElementById("filtroCategoria").value = "";

    // Resetear slider al máximo
    const slider = document.getElementById("filtroMontoMax");
    const valorSlider = document.getElementById("valorMontoMax");

    const maxMonto = Math.max(...data.transacciones.map(t => t.montoTransac));
    slider.value = maxMonto;
    valorSlider.textContent = maxMonto;

    // Resetear fechas
    document.getElementById("filtroFechaDesde").value = "";
    document.getElementById("filtroFechaHasta").value = "";

    // Mostrar todas las transacciones
    mostrarTransacciones(data.transacciones);

}

// Configurar el slider de monto máximo según las transacciones cargadas
function configurarSlider() {
    const slider = document.getElementById("filtroMontoMax");
    const valorSlider = document.getElementById("valorMontoMax");

    if (data.transacciones.length === 0) return;

    const maxMonto = Math.max(...data.transacciones.map(t => t.montoTransac));

    slider.max = maxMonto;
    slider.value = maxMonto;
    valorSlider.textContent = maxMonto.toFixed(2);
}



// ------------------ EVENTOS ------------------
// Cargar categorías y mostrar transacciones al cargar la página
document.addEventListener("DOMContentLoaded", () => {

  // Bloquear fechas futuras
  const hoy = new Date().toISOString().split("T")[0];
  document.getElementById("filtroFechaDesde").setAttribute("max", hoy);
  document.getElementById("filtroFechaHasta").setAttribute("max", hoy);

  // Cargar datos desde API
  cargarTransaccionesDesdeAPI();

  // Slider cambia en tiempo real
  document.getElementById("filtroMontoMax").addEventListener("input", aplicarFiltros);

  // Filtros automáticos
  document.getElementById("filtroTipo").addEventListener("change", aplicarFiltros);
  document.getElementById("filtroCategoria").addEventListener("change", aplicarFiltros);

  // Filtros manuales (fechas)
  document.getElementById("aplicarFiltrosBtn").addEventListener("click", aplicarFiltros);

  // Resetear filtros
  document.getElementById("limpiarBtn").addEventListener("click", resetearFiltros);
});
