const API_URL = 'https://localhost:7258/api'; // Reemplacen con la de su computadora

const data = {
  transacciones: [
    {
      fechaTransac: "2025-01-31T00:00:00",
      montoTransac: 2000,
      descripcionTransac: "Pago de salario mensual",
      nombreCateg: "Salario"
    },
    {
      fechaTransac: "2025-01-15T00:00:00",
      montoTransac: 150.75,
      descripcionTransac: "Compra de supermercado",
      nombreCateg: "Alimentación"
    },

    {
      fechaTransac: "2025-01-15T00:00:00",
      montoTransac: 150.75,
      descripcionTransac: "Compra de supermercado",
      nombreCateg: "Alimentación"
    },

    {
      fechaTransac: "2025-01-15T00:00:00",
      montoTransac: 150.75,
      descripcionTransac: "Compra de supermercado",
      nombreCateg: "Alimentación"
    }
  ]
};

const contenedor = document.getElementById("lista-transacciones");

//Para cada registro de transaccion, crear un div con su informacion y lo agrega al contenedor
if (data.transacciones) data.transacciones.forEach(t => {
  contenedor.innerHTML += `
    <div class="item">
      <div class= "item-encabezado">
        <h3>${t.nombreCateg}</h3>
        <button class="btn-opciones"></button>
      </div>
      
      <p>${t.descripcionTransac}</p>
      <span>Monto: $${t.montoTransac}</span><br>
      <span>Fecha: ${new Date(t.fechaTransac).toLocaleDateString()}</span>
    </div>
  `;
}); else {
  contenedor.innerHTML = "<p>No hay transacciones para mostrar.</p>";
}
