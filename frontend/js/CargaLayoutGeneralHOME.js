/*Función para darle layout a todas las pantallas que tienen los mismos elementos*/
cargarLayout('header', '../components/headerHOME.html'); 
cargarLayout('footer', '../components/footerHOME.html');

function cargarLayout(id, url) { /*Recibe como parámetros el id y la dirección del recurso*/ 
    fetch(url) 
        .then(response => response.text())
        .then(html => {
          document.getElementById(id).innerHTML = html;
        })
        .catch(err => console.error("Error cargando ${id}:", err)); 
}

