const API_URL = 'https://localhost:7258/api';

const token = localStorage.getItem("token");

async function cargarTransaccionesDesdeAPI() {
    try {
        const token = localStorage.getItem("token");

        if (!token) {
            console.error("No hay token. El usuario no está autenticado.");
            return;
        }

        const respuesta = await fetch(`${API_URL}/Transaccion/MisTransacciones`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (respuesta.status === 401) {
            mostrarPopup("Tu sesión expiró. Inicia sesión de nuevo.");
            window.location.href = "../pages/autenticacion.html";
            return;
        }

        if (!respuesta.ok) {
            throw new Error("Error al obtener transacciones");
        }

        const resultado = await respuesta.json();
        data = resultado;

        mostrarTransacciones(data.transacciones);
        await cargarCategorias();
        await cargarCategoriasCombobox("categoriaTransaccion", "tipoTransaccion");
        configurarSlider();
        actualizarEstadoBotonesFiltros();

    } catch (error) {
        console.error("Error al cargar las transacciones:", error);
        document.getElementById("lista-transacciones").innerHTML =
            "<p>Error al cargar transacciones.</p>";
    }
}

async function crearCategoria() {
    const nombre = document.getElementById("nombreCategoria").value.trim();
    const tipo = document.getElementById("tipoCategoria").value;

    if (!nombre || !tipo) {
        mostrarPopup("Por favor completa los campos requeridos.");
        return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
        mostrarPopup("No estás autenticado. Por favor inicia sesión.");
        return;
    }

    try {
        const envio = {
            nombreCateg: nombre,
            tipoCateg: tipo
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
            mostrarPopup("Tu sesión expiró. Inicia sesión de nuevo.");
            window.location.href = "../pages/autenticacion.html";
            return;
        }

        if (!respuesta.ok) {
            const errorData = await respuesta.json();
            mostrarPopup(`Error: ${errorData.mensaje || "No se pudo crear la categoría"}`);
            return;
        }

        mostrarPopup("Categoría creada exitosamente.");
        cerrarPopupCategoria();
        
        // Recargar transacciones para actualizar la lista de categorías
        cargarTransaccionesDesdeAPI();

    } catch (error) {
        console.error("Error al crear categoría:", error);
        mostrarPopup("Error al crear la categoría. Por favor intenta de nuevo.");
    }
}

async function crearTransaccion() {
    const categoria = document.getElementById("categoriaTransaccion").value;
    const tipo = document.getElementById("tipoTransaccion").value;
    const descripcion = document.getElementById("descripcionTransaccion").value.trim();
    const monto = parseFloat(document.getElementById("montoTransaccion").value);
    const fecha = document.getElementById("fechaTransaccion").value;

    if (!categoria || !tipo || !descripcion || !monto || !fecha) {
        mostrarPopup("Por favor completa todos los campos.");
        return;
    }

    if (monto <= 0) {
        mostrarPopup("El monto debe ser mayor a 0.");
        return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
        mostrarPopup("No estás autenticado. Por favor inicia sesión.");
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
            mostrarPopup("Tu sesión expiró. Inicia sesión de nuevo.");
            window.location.href = "../pages/autenticacion.html";
            return;
        }

        if (!respuesta.ok) {
            const errorData = await respuesta.json();
            mostrarPopup(`Error: ${errorData.mensaje || "No se pudo crear la transacción"}`);
            return;
        }

        mostrarPopup("Transacción creada exitosamente.");
        cerrarPopupTransaccion();
        
        // Recargar transacciones para actualizar la lista
        cargarTransaccionesDesdeAPI();

    } catch (error) {
        console.error("Error al crear transacción:", error);
        mostrarPopup("Error al crear la transacción. Por favor intenta de nuevo.");
    }
}

async function eliminarTransaccion(idTransaccion) {
    const token = localStorage.getItem("token");

    if (!token) {
        mostrarPopup("No estás autenticado. Por favor inicia sesión.");
        return;
    }

    try {
        const respuesta = await fetch(`${API_URL}/Transaccion/EliminarTransaccion/${idTransaccion}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        if (respuesta.status === 401) {
            mostrarPopup("Tu sesión expiró. Inicia sesión de nuevo.");
            window.location.href = "../pages/autenticacion.html";
            return;
        }
        
        if (!respuesta.ok) {
            const errorData = await respuesta.json();
            mostrarPopup(`Error: ${errorData.mensaje || "No se pudo eliminar la transacción"}`);
            return;
        }
        
        mostrarPopup("Transacción eliminada exitosamente.");
        cargarTransaccionesDesdeAPI();
    } catch (error) {
        console.error("Error al eliminar transacción:", error);
        mostrarPopup("Error al eliminar la transacción. Por favor intenta de nuevo.");
    }
}

async function actualizarTransaccion(idTransaccion) {
    // Obtener valores directamente (sin paréntesis en .value)
    const descripcion = document.getElementById("descripcionModificar").value.trim();
    const monto = parseFloat(document.getElementById("montoModificar").value);
    const fecha = document.getElementById("fechaModificar").value;
    const categoria = document.getElementById("categoriaModificar").value;
    
    if (!descripcion || !monto || !fecha || !categoria) {
        mostrarPopup("Por favor completa todos los campos.");
        return;
    }

    if (monto <= 0) {
        mostrarPopup("El monto debe ser mayor a 0.");
        return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
        mostrarPopup("No estás autenticado. Por favor inicia sesión.");
        return;
    }

    try {
        const envio = {
            descripcionTransac: descripcion,
            montoTransac: monto,
            fechaTransac: fecha,
            nombreCateg: categoria
        };
        
        const respuesta = await fetch(`${API_URL}/Transaccion/ActualizarTransaccion/${idTransaccion}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(envio)
        });

        if (respuesta.status === 401) {
            mostrarPopup("Tu sesión expiró. Inicia sesión de nuevo.");
            window.location.href = "../pages/autenticacion.html";
            return;
        }

        if (!respuesta.ok) {
            const errorData = await respuesta.json();
            mostrarPopup(`Error: ${errorData.mensaje || "No se pudo actualizar la transacción"}`);
            return;
        }

        mostrarPopup("Transacción actualizada exitosamente.");
        cerrarPopupModificarTransaccion();
        
        // Recargar transacciones para actualizar la lista
        cargarTransaccionesDesdeAPI();

    } catch (error) {
        console.error("Error al actualizar transacción:", error);
        mostrarPopup("Error al actualizar la transacción. Por favor intenta de nuevo.");
    }
}