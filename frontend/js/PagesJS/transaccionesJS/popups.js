// ==================== FUNCIONES PARA ABRIR Y CERRAR POPUPS ====================

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

function cerrarPopupTransaccion() {
    document.getElementById("popupTransaccion").classList.add("hidden");
    document.getElementById("formTransaccion").reset();
}

// Cerrar popup cuando se hace clic fuera del contenido
document.addEventListener("click", (e) => {
    const popupCategoria = document.getElementById("popupCategoria");
    const popupTransaccion = document.getElementById("popupTransaccion");

    if (e.target === popupCategoria) {
        cerrarPopupCategoria();
    }
    if (e.target === popupTransaccion) {
        cerrarPopupTransaccion();
    }
});

// Cerrar popups con la tecla ESC
document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        cerrarPopupCategoria();
        cerrarPopupTransaccion();
    }
});

// ==================== MANEJAR ENVÍO DE FORMULARIOS ====================

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

    // Event listener para el cambio de tipo en la transacción
    const selectTipo = document.getElementById("tipoTransaccion");
    if (selectTipo) {
        selectTipo.addEventListener("change", filtrarCategoriasPorTipo);
    }
});

// ==================== FUNCIONES PARA LLAMAR A LA API ====================

async function crearCategoria() {
    const nombre = document.getElementById("nombreCategoria").value.trim();
    const tipo = document.getElementById("tipoCategoria").value;

    if (!nombre || !tipo) {
        alert("Por favor completa los campos requeridos.");
        return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
        alert("No estás autenticado. Por favor inicia sesión.");
        return;
    }

    try {
        const envio = {
            nombre: nombre,
            tipo: tipo
        };

        const respuesta = await fetch(`${API_URL}/Categoria/AgregarCategoria`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(envio)
        });

        if (respuesta.status === 401) {
            alert("Tu sesión expiró. Inicia sesión de nuevo.");
            window.location.href = "autenticacion.html";
            return;
        }

        if (!respuesta.ok) {
            const errorData = await respuesta.json();
            alert(`Error: ${errorData.mensaje || "No se pudo crear la categoría"}`);
            return;
        }

        alert("Categoría creada exitosamente.");
        cerrarPopupCategoria();
        
        // Recargar transacciones para actualizar la lista de categorías
        cargarTransaccionesDesdeAPI();

    } catch (error) {
        console.error("Error al crear categoría:", error);
        alert("Error al crear la categoría. Por favor intenta de nuevo.");
    }
}

async function crearTransaccion() {
    const categoria = document.getElementById("categoriaTransaccion").value;
    const tipo = document.getElementById("tipoTransaccion").value;
    const descripcion = document.getElementById("descripcionTransaccion").value.trim();
    const monto = parseFloat(document.getElementById("montoTransaccion").value);
    const fecha = document.getElementById("fechaTransaccion").value;

    if (!categoria || !tipo || !descripcion || !monto || !fecha) {
        alert("Por favor completa todos los campos.");
        return;
    }

    if (monto <= 0) {
        alert("El monto debe ser mayor a 0.");
        return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
        alert("No estás autenticado. Por favor inicia sesión.");
        return;
    }

    try {
        const envio = {
            nombreCateg: categoria,
            descripcionTransac: descripcion,
            montoTransac: monto,
            fechaTransac: fecha
        };

        const respuesta = await fetch(`${API_URL}/Transaccion/RegistrarTransaccion`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(envio)
        });

        if (respuesta.status === 401) {
            alert("Tu sesión expiró. Inicia sesión de nuevo.");
            window.location.href = "autenticacion.html";
            return;
        }

        if (!respuesta.ok) {
            const errorData = await respuesta.json();
            alert(`Error: ${errorData.mensaje || "No se pudo crear la transacción"}`);
            return;
        }

        alert("Transacción creada exitosamente.");
        cerrarPopupTransaccion();
        
        // Recargar transacciones para actualizar la lista
        cargarTransaccionesDesdeAPI();

    } catch (error) {
        console.error("Error al crear transacción:", error);
        alert("Error al crear la transacción. Por favor intenta de nuevo.");
    }
}

// ==================== CARGAR CATEGORÍAS EN EL SELECT ====================

async function cargarCategoriasEnSelect() {
    const selectCategoria = document.getElementById("categoriaTransaccion");
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
        
        // Filtrar categorías por el tipo seleccionado (si hay uno)
        filtrarCategoriasPorTipo();

    } catch (error) {
        console.error("Error al cargar categorías en el select:", error);
    }
}

// Función para filtrar categorías según el tipo seleccionado
function filtrarCategoriasPorTipo() {
    const selectCategoria = document.getElementById("categoriaTransaccion");
    const tipoSeleccionado = document.getElementById("tipoTransaccion").value;

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
