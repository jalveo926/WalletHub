//Función para mostrar el popup
//Fallback inmediato para evitar errores si se llama mostrarPopup antes de que se cargue el HTML del popup
window.mostrarPopup = function(mensaje) {
    alert(mensaje);
};

window.addEventListener("DOMContentLoaded", () => {
    //Ruta relativa correcta desde pages/transaccion.html hacia frontend/components/popup.html
    fetch("../components/popup.html")
        .then(res => {
            if (!res.ok) throw new Error(`No se pudo cargar popup.html: ${res.status}`);
            return res.text();
        })
        .then(html => {
            document.getElementById("popup-container").innerHTML = html;

            const popup = document.querySelector("#popup-container .popup-general");
            const popupText = document.querySelector("#popup-text");
            const popupBtn = document.querySelector("#popup-btn");

            // Sobrescribimos el fallback con la implementación visual
            window.mostrarPopup = function(mensaje) {
                popupText.textContent = mensaje;
                popup.style.display = "flex";
            };

            popupBtn.addEventListener("click", () => {
                popup.style.display = "none";
            });
        })
        .catch(err => {
            console.error("Error cargando popup:", err);
            //Si falla el fetch, el fallback (alert) está disponible.
        });
});
