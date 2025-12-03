/*Selecciona los contenedres de los dos formularios*/ 
const loginCont = document.querySelector('.login-contenedor');
const regCont = document.querySelector('.registro-contenedor');

/*Cuando se toca el botón "¿No tienes cuenta? Regístrate"*/
document.getElementById('mostrar-registro').addEventListener('click', () => {
    loginCont.classList.add('oculto'); //Oculta el login
    regCont.classList.remove('oculto'); //Muestra el registro
});

/*Cuando se toca el botón "¿Ya tienes cuenta? Inicia sesión"*/
document.getElementById('mostrar-login').addEventListener('click', () => {
    regCont.classList.add('oculto'); //Oculta el registro
    loginCont.classList.remove('oculto'); //Muestra el login
});


//Toggle para mostrar la contraseña y ocultarla 
document.querySelectorAll(".toggle-password").forEach(icon => {
    icon.addEventListener("click", () => {
        const inputId = icon.getAttribute("data-target");
        const input = document.getElementById(inputId);

        const showing = input.type === "text"; 
        input.type = showing ? "password" : "text"; //Si showing es true, cambia a password. Si es false, cambia a text

        icon.style.backgroundImage = showing
            ? "url('../assets/icons/eye-off.svg')" //Si showing es true, muestra el ojo cerrado cuando se oculta la contraseña
            : "url('../assets/icons/eye.svg')"; //Si showing es false, muestra el ojo abierto cuando se muestra la contraseña
    });
});


function GuardarToken(token, usuario) {
    localStorage.setItem('token', token);
    localStorage.setItem('usuario', JSON.stringify(usuario));
}

function obtenerToken() {
    return localStorage.getItem('token');
}

function estaLogueado() {
    const token = obtenerToken();
    return token !== null;
}

//Mensaje de error login
const mensajeLogin = document.getElementById('mensaje-login');
 
function mostrarMensajeLogin(texto) {
    mensajeLogin.style.display = 'block';
    mensajeLogin.textContent = texto;
    mensajeLogin.className = 'mensaje mensaje-error'; 
}

//Mensaje de error registro
const mensajeReg = document.getElementById('mensaje-reg');
 
function mostrarMensajeRegistro(texto) {
    mensajeReg.style.display = 'block';
    mensajeReg.textContent = texto;
    mensajeReg.className = 'mensaje mensaje-error';
}
