// Variable global para almacenar las transacciones cargadas desde la API
let data = {
    mensaje: "",
    transacciones: []
};

//CARGAR CATEGORÍAS

function cargarCategorias() {
    const filtroCategoria = document.getElementById("filtroCategoria");

    //new set nos ayuda a obtener solo los valores únicos de un array y ... los desglosa en un nuevo array
    const categoriasUnicas = [...new Set(data.transacciones.map(t => t.nombreCateg))];

    //cat es cada categoría única que se añade al dropdown filtroCategoria
    categoriasUnicas.forEach(cat => {
        const option = document.createElement("option");
        option.value = cat;
        option.textContent = cat;
        filtroCategoria.appendChild(option);
    });
}


//MOSTRAR TRANSACCIONES
//Aqui se crean tambien los botones de modificiar y eliminar
function mostrarTransacciones(lista) {
    //En lista se reciben las transacciones ya sean normales o filtradas

    const contenedor = document.getElementById("lista-transacciones");
    contenedor.innerHTML = "";

    if (lista.length === 0) {
        contenedor.innerHTML = "<p>No hay transacciones para mostrar.</p>";
        return;
    }

    lista.forEach(t => {
        // Determinar clase CSS según el tipo de transacción
        const tipoClase = t.tipoCategoria === "Ingreso" ? "ingreso" : "gasto";

        // Crear el HTML de cada item transacción (tarjetas)
        contenedor.innerHTML += `
        <div class="item ${tipoClase}">
            <div class="item-encabezado">
                <h3>${t.nombreCateg}</h3>
                <div class="btn-opciones-container">
                    <button class="btn-opciones" data-id="${t.idTransaccion}"></button>
                    <div class="menu-opciones hidden" data-id="${t.idTransaccion}">
                        <button class="opcion-modificar" data-id="${t.idTransaccion}">Modificar</button>
                        <button class="opcion-eliminar" data-id="${t.idTransaccion}">Eliminar</button>
                    </div>
                </div>
            </div>

            <p>${t.descripcionTransac}</p>
            <span>Monto: $${t.montoTransac}</span><br>
            <span>Fecha: ${new Date(t.fechaTransac).toLocaleDateString()}</span> 
        </div>
    `;
    });

}

//APLICAR FILTROS

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
            mostrarPopup("La fecha 'Hasta' no puede ser menor que 'Desde'.");
            return;
        }
    }

    // Filtrar por tipo

    //filter nos ayuda a crear un nuevo array con los elementos que cumplan la condición
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
    //filter nos ayuda a crear un nuevo array con los elementos que cumplan la condición
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

    //Math.max nos ayuda a obtener el valor máximo de un array y los ... desglosan el array en valores individuales
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

    //Math.max nos ayuda a obtener el valor máximo de un array y los ... desglosan el array en valores individuales
    const maxMonto = Math.max(...data.transacciones.map(t => t.montoTransac));

    //slider nos ayuda a establecer el valor máximo del slider al monto máximo encontrado
    slider.max = maxMonto;
    slider.value = maxMonto;

    //valorSlider nos ayuda a mostrar el valor máximo al lado del slider y toFixed(2) limita a 2 decimales
    valorSlider.textContent = maxMonto.toFixed(2);
}

// Actualizar estado del botón de filtros según disponibilidad de transacciones
function actualizarEstadoBotonesFiltros() {
    const btnAplicarFiltros = document.getElementById("aplicarFiltrosBtn");
    const btnLimpiar = document.getElementById("limpiarBtn");
    const tieneTransacciones = data.transacciones.length > 0;

    // Deshabilitar botones si no hay transacciones
    btnAplicarFiltros.disabled = !tieneTransacciones;
    btnLimpiar.disabled = !tieneTransacciones;
}

//EVENTOS

// Cargar categorías y mostrar transacciones al cargar la página
//Configuracion de elementos dinámicos y sus eventos
document.addEventListener("DOMContentLoaded", () => {

    // Bloquear fechas futuras
    const hoy = new Date().toISOString().split("T")[0];
    document.getElementById("filtroFechaDesde").setAttribute("max", hoy);
    document.getElementById("filtroFechaHasta").setAttribute("max", hoy);

    // Deshabilitar botones inicialmente
    actualizarEstadoBotonesFiltros();

    // Cargar datos desde API
    cargarTransaccionesDesdeAPI();

    // Slider cambia en tiempo real
    document.getElementById("filtroMontoMax").addEventListener("input", (e) => {
        document.getElementById("valorMontoMax").textContent = parseFloat(e.target.value).toFixed(2);
        aplicarFiltros();
    });

    // Filtros automáticos
    document.getElementById("filtroTipo").addEventListener("change", aplicarFiltros);
    document.getElementById("filtroCategoria").addEventListener("change", aplicarFiltros);

    // Filtros manuales (fechas)
    document.getElementById("aplicarFiltrosBtn").addEventListener("click", aplicarFiltros);

    // Resetear filtros
    document.getElementById("limpiarBtn").addEventListener("click", resetearFiltros);

    // Event delegation para abrir/cerrar menú de opciones
    document.addEventListener("click", (e) => {
        // Cerrar todos los menús si se hace clic fuera
        if (!e.target.closest(".btn-opciones-container")) {
            document.querySelectorAll(".menu-opciones").forEach(menu => {
                menu.classList.add("hidden");
            });
        }

        // Abrir/cerrar menú al hacer clic en btn-opciones
        if (e.target.closest(".btn-opciones")) {
            e.stopPropagation();
            const btn = e.target.closest(".btn-opciones");
            const container = btn.closest(".btn-opciones-container");
            const menu = container.querySelector(".menu-opciones");

            // Cerrar otros menús
            document.querySelectorAll(".menu-opciones").forEach(m => {
                if (m !== menu) {
                    m.classList.add("hidden");
                }
            });

            // Toggle del menú actual
            menu.classList.toggle("hidden");
        }
        // Acceder al botón Modificar
        if (e.target.closest(".opcion-modificar")) {
            const idTransaccion = e.target.closest(".opcion-modificar").getAttribute("data-id"); //Obtener ID de la transacción
            console.log("Modificar transacción:", idTransaccion);
            abrirPopupModificarTransaccion(idTransaccion);

        }

        // Acceder al botón Eliminar
        if (e.target.closest(".opcion-eliminar")) {
            const idTransaccion = e.target.closest(".opcion-eliminar").getAttribute("data-id"); //Obtener ID de la transacción
            mostrarConfirmacion("¿Estás seguro de que deseas eliminar esta transacción?", (confirmar) => {
                if (confirmar) {
                    eliminarTransaccion(idTransaccion);
                    console.log("Eliminar transacción:", idTransaccion);
                }
            });
        }
    });
});