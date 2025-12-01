const API_URL = 'https://localhost:7258/api'; // Cambiar cuando uses fetch real

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
                'Authorization': `Bearer ${token}` // Agrega el token en el encabezado Authorization
            }
        });

        if (respuesta.status === 401) {
            alert("Tu sesión expiró. Inicia sesión de nuevo.");
            window.location.href = "autenticacion.html";
        return;
}

        if (!respuesta.ok) {
            throw new Error("Error al obtener transacciones");
        }

        const resultado = await respuesta.json();

        data = resultado;

        mostrarTransacciones(data.transacciones);
        await cargarCategorias();
        await cargarCategoriasEnSelect();
        configurarSlider();
        actualizarEstadoBotonesFiltros();

    } catch (error) {
        console.error("Error al cargar las transacciones:", error);
        document.getElementById("lista-transacciones").innerHTML =
            "<p>Error al cargar transacciones.</p>";
    }
}
