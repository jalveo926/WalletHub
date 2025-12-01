//Función para mostrar el popup
//Carga el popup desde el archivo externo
fetch("../components/popup.html")
    .then(res => res.text())
    .then(html => {
        //Insertar el popup en la página
        document.getElementById("popup").innerHTML = html;

        //Activar los botones y funciones una vez insertado el HTML
        const popup = document.querySelector("#popup .popup");
        const popupText = document.getElementById("popup-text");
        const popupBtn = document.getElementById("popup-btn");

        window.mostrarPopup = function(mensaje) {
            popupText.textContent = mensaje;
            popup.style.display = "flex";
        };

        //Botón OK
        popupBtn.addEventListener("click", () => {
            popup.style.display = "none";
        });
    });