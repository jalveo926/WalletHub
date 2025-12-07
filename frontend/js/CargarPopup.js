//Si el popup no se ha cargado aún, mostrar un alert como fallback
window.mostrarPopup = function(mensaje) {
    alert(mensaje);
};

window.addEventListener("DOMContentLoaded", () => {
    fetch("../components/popup.html") //Carga el HTML del popup desde un archivo externo
        .then(res => {
            if (!res.ok) throw new Error(`No se pudo cargar popup.html: ${res.status}`); 
            return res.text();
        })
        .then(html => {
            document.getElementById("popup-container").innerHTML = html; //Insertar el HTML cargado en el contenedor del popup

            //POPUP DE MENSAJE
            
            const popup = document.querySelector("#popup-container #popup-mensaje"); //Contenedor del popup
            const popupText = popup.querySelector("#popup-text"); //Texto del popup
            const popupBtn = popup.querySelector("#popup-btn"); //Botón de cierre del popup

            //Función para mostrar el popup con un mensaje
            window.mostrarPopup = function(mensaje) {
                popupText.textContent = mensaje;
                popup.style.display = "flex"; //Mostrar el popup
            };

            //Cerrar el popup al hacer clic en el botón
            popupBtn.addEventListener("click", () => {
                popup.style.display = "none";
            });

            //POPUP DE CONFIRMACIÓN
            
            const popupConfirm = document.querySelector("#popup-container #popup-confirmacion");
            const popupConfirmText = popupConfirm.querySelector("#popup-confirm-text");
            const btnAceptar = popupConfirm.querySelector("#popup-aceptar-btn");
            const btnCancelar = popupConfirm.querySelector("#popup-cancelar-btn");

            //Función para mostrar el popup de confirmación
            //callback: función que se ejecuta con true (aceptar) o false (cancelar)
            window.mostrarConfirmacion = function(mensaje, callback) {
                popupConfirmText.textContent = mensaje;
                popupConfirm.style.display = "flex";

                //Eliminar event listeners previos para evitar múltiples llamadas
                btnAceptar.onclick = null;
                btnCancelar.onclick = null;

                btnAceptar.onclick = () => {
                    popupConfirm.style.display = "none";
                    callback(true);
                };

                btnCancelar.onclick = () => {
                    popupConfirm.style.display = "none";
                    callback(false);
                };
            };
        })
        .catch(err => {
            console.error("Error cargando popup:", err); //Manejo de errores si el fetch falla
        });
});
