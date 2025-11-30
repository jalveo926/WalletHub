const API_URL = 'https://localhost:7258/api'; // Cambiar cuando uses fetch real

const data = {
  mensaje: "Transacciones obtenidas exitosamente.",
  transacciones: [
    {
      fechaTransac: "2025-01-31T00:00:00",
      montoTransac: 2000,
      descripcionTransac: "Pago de salario mensual",
      nombreCateg: "Salario",
      tipoCategoria: "Ingreso"
    },
    {
      fechaTransac: "2025-01-15T00:00:00",
      montoTransac: 150.75,
      descripcionTransac: "Compra de supermercado",
      nombreCateg: "Alimentación",
      tipoCategoria: "Gasto"
    },
    {
      fechaTransac: "2024-01-15T00:00:00",
      montoTransac: 150.75,
      descripcionTransac: "Compra de supermercado",
      nombreCateg: "Alimentación",
      tipoCategoria: "Gasto"
    },
    {
      fechaTransac: "2025-01-31T00:00:00",
      montoTransac: 3000,
      descripcionTransac: "Pago de salario mensual",
      nombreCateg: "Salario",
      tipoCategoria: "Ingreso"
    },
    {
      fechaTransac: "2025-01-31T00:00:00",
      montoTransac: 3000.23,
      descripcionTransac: "Pago de salario mensual",
      nombreCateg: "Salario",
      tipoCategoria: "Ingreso"
    }
  ]
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


// ------------------ EVENTOS ------------------
// Cargar categorías y mostrar transacciones al cargar la página
document.addEventListener("DOMContentLoaded", () => {

  // Configurar slider según el monto máximo real
  const maxMonto = Math.max(...data.transacciones.map(t => t.montoTransac));
  const slider = document.getElementById("filtroMontoMax");
  const valorSlider = document.getElementById("valorMontoMax");

  slider.max = maxMonto;
  slider.value = maxMonto;
  valorSlider.textContent = maxMonto.toFixed(2);

   // Actualizar valor del slider en tiempo real
  slider.addEventListener("input", () => {
      const v = Number(slider.value);
      valorSlider.textContent = v.toFixed(2);
      aplicarFiltros();
  });


  // Bloquear fechas futuras
  const hoy = new Date().toISOString().split("T")[0];
  document.getElementById("filtroFechaDesde").setAttribute("max", hoy);
  document.getElementById("filtroFechaHasta").setAttribute("max", hoy);

  // Filtros automáticos (tipo y categoría)
  document.getElementById("filtroTipo").addEventListener("change", aplicarFiltros);
  document.getElementById("filtroCategoria").addEventListener("change", aplicarFiltros);

  // Filtros manuales (solo fechas)
  document.getElementById("aplicarFiltrosBtn")
      .addEventListener("click", aplicarFiltros);
  
  cargarCategorias();
  mostrarTransacciones(data.transacciones);
});