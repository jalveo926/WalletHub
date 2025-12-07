//FUNCIONES PARA ABRIR Y CERRAR POPUPS

// Variable global para almacenar todas las categorías
let todasLasCategorias = [];

function abrirPopupCategoria() {
    document.getElementById("popupCategoria").classList.remove("hidden");
}

function cerrarPopupCategoria() {
    document.getElementById("popupCategoria").classList.add("hidden");
    document.getElementById("formCategoria").reset();
}

function abrirPopupTransaccion() {
    document.getElementById("popupTransaccion").classList.remove("hidden");
    // Establecer la fecha de hoy por defecto
    const hoy = new Date().toISOString().split("T")[0];
    document.getElementById("fechaTransaccion").value = hoy;
}

async function abrirPopupModificarTransaccion(idTransaccion) {
    // Buscar la transacción en el array global
    const transaccion = data.transacciones.find(t => t.idTransaccion == idTransaccion);
    
    if (!transaccion) {
        mostrarPopup("No se encontró la transacción");
        return;
    }

    // Cargar las categorías primero
    await cargarCategoriasCombobox("categoriaModificar", "tipoModificar");
    
    // Llenar el formulario con los datos de la transacción
    document.getElementById("tipoModificar").value = transaccion.tipoCategoria;
    
    // Filtrar categorías según el tipo
    filtrarCategoriasPorTipo("categoriaModificar", "tipoModificar");
    
    // Establecer la categoría seleccionada
    document.getElementById("categoriaModificar").value = transaccion.nombreCateg;
    document.getElementById("descripcionModificar").value = transaccion.descripcionTransac;
    document.getElementById("montoModificar").value = transaccion.montoTransac;
    
    // Formatear la fecha correctamente (solo la parte de la fecha, sin hora)
    const fecha = new Date(transaccion.fechaTransac);
    const fechaFormateada = fecha.toISOString().split("T")[0];
    document.getElementById("fechaModificar").value = fechaFormateada;
    
    // Guardar el ID de la transacción en un atributo del formulario para usarlo después
    document.getElementById("formModificarTransaccion").setAttribute("data-id-transaccion", idTransaccion);
    
    // Mostrar el popup
    document.getElementById("popupModificarTransaccion").classList.remove("hidden");
}

function cerrarPopupModificarTransaccion() {
    document.getElementById("popupModificarTransaccion").classList.add("hidden");
    document.getElementById("formModificarTransaccion").reset();
    document.getElementById("formModificarTransaccion").removeAttribute("data-id-transaccion");
}

function cerrarPopupTransaccion() {
    document.getElementById("popupTransaccion").classList.add("hidden");
    document.getElementById("formTransaccion").reset();
}

// Cerrar popup cuando se hace clic fuera del contenido
document.addEventListener("click", (e) => {
    const popupCategoria = document.getElementById("popupCategoria");
    const popupTransaccion = document.getElementById("popupTransaccion");
    const popupModificar = document.getElementById("popupModificarTransaccion");

    if (e.target === popupCategoria) {
        cerrarPopupCategoria();
    }
    if (e.target === popupTransaccion) {
        cerrarPopupTransaccion();
    }
    if (e.target === popupModificar) {
        cerrarPopupModificarTransaccion();
    }
});

// Cerrar popups con la tecla ESC
document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        cerrarPopupCategoria();
        cerrarPopupTransaccion();
        cerrarPopupModificarTransaccion();
    }
});

// MANEJAR ENVÍO DE FORMULARIOS 

// Evento para crear categoría
document.addEventListener("DOMContentLoaded", () => {
    // Botones para abrir popups
    const btnCrearCategoria = document.getElementById("crearCategoriaBtn");
    const btnCrearTransaccion = document.getElementById("crearTransaccionBtn");

    if (btnCrearCategoria) {
        btnCrearCategoria.addEventListener("click", abrirPopupCategoria);
    }

    if (btnCrearTransaccion) {
        btnCrearTransaccion.addEventListener("click", abrirPopupTransaccion);
    }

    // Formulario de categoría
    const formCategoria = document.getElementById("formCategoria");
    if (formCategoria) {
        formCategoria.addEventListener("submit", async (e) => {
            e.preventDefault();
            await crearCategoria();
        });
    }

    // Formulario de transacción
    const formTransaccion = document.getElementById("formTransaccion");
    if (formTransaccion) {
        formTransaccion.addEventListener("submit", async (e) => {
            e.preventDefault();
            await crearTransaccion();
        });
    }

    // Formulario de modificar transacción
    const formModificar = document.getElementById("formModificarTransaccion");
    if (formModificar) {
        formModificar.addEventListener("submit", async (e) => {
            e.preventDefault();
            const idTransaccion = formModificar.getAttribute("data-id-transaccion");
            await actualizarTransaccion(idTransaccion);
        });
    }

    // Event listener para el cambio de tipo en la transacción (CREAR)
    const selectTipo = document.getElementById("tipoTransaccion");
    if (selectTipo) {
        selectTipo.addEventListener("change", () => {
            filtrarCategoriasPorTipo("categoriaTransaccion", "tipoTransaccion");
        });
    }

    // Event listener para el cambio de tipo en la transacción (MODIFICAR)
    const selectTipoModificar = document.getElementById("tipoModificar");
    if (selectTipoModificar) {
        selectTipoModificar.addEventListener("change", () => {
            filtrarCategoriasPorTipo("categoriaModificar", "tipoModificar");
        });
    }
});


// Cargar categorías en un combobox específico
async function cargarCategoriasCombobox(idComboboxCategoria, idComboBoxTipoCat) {
    const selectCategoria = document.getElementById(idComboboxCategoria);
    const token = localStorage.getItem("token");

    if (!token) {
        console.error("No hay token. El usuario no está autenticado.");
        return;
    }

    // Limpiar opciones anteriores (excepto la primera)
    while (selectCategoria.options.length > 1) {
        selectCategoria.remove(1);
    }

    try {
        const respuesta = await fetch(`${API_URL}/Categoria/CategoriasGlobales`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        if (respuesta.status === 401) {
            console.error("Sesión expirada al cargar categorías");
            return;
        }

        if (!respuesta.ok) {
            throw new Error("Error al obtener categorías");
        }

        const resultado = await respuesta.json();
        todasLasCategorias = resultado.categorias || [];
        
        // Filtrar categorías por el tipo seleccionado 
        filtrarCategoriasPorTipo(idComboboxCategoria, idComboBoxTipoCat);

    } catch (error) {
        console.error("Error al cargar categorías en el select:", error);
    }
}

// Función para filtrar categorías según el tipo seleccionado
function filtrarCategoriasPorTipo(idComboboxCategoria, idComboBoxTipoCat) {
    const selectCategoria = document.getElementById(idComboboxCategoria);
    const tipoSeleccionado = document.getElementById(idComboBoxTipoCat).value;

    // Limpiar opciones anteriores (excepto la primera)
    while (selectCategoria.options.length > 1) {
        selectCategoria.remove(1);
    }

    if (!tipoSeleccionado) {
        return; // Si no hay tipo seleccionado, no mostrar categorías
    }

    // Filtrar categorías por el tipo seleccionado
    const categoriasFiltradas = todasLasCategorias.filter(cat => cat.tipoCateg === tipoSeleccionado);

    // Agregar opciones de categorías filtradas
    categoriasFiltradas.forEach(cat => {
        const option = document.createElement("option");
        option.value = cat.nombreCateg;
        option.textContent = cat.nombreCateg;
        selectCategoria.appendChild(option);
    });
}