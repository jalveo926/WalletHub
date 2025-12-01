//Función para mostrar el popup
window.addEventListener("DOMContentLoaded", () => {
    // Cargar HTML del popup
    fetch("../components/popup.html")
        .then(res => res.text())
        .then(html => {
            document.getElementById("popup-general").innerHTML = html;

            const popup = document.querySelector("#popup-general .popup-general");
            const popupText = popup.querySelector("#popup-text");
            const popupBtn = popup.querySelector("#popup-btn");

            window.mostrarPopup = function(mensaje) {
                popupText.textContent = mensaje;
                popup.style.display = "flex";
            };

            popupBtn.addEventListener("click", () => {
                popup.style.display = "none";
            });
        });
});