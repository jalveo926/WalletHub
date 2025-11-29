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