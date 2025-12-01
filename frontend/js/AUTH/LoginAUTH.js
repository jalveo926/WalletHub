const API_URL = 'https://localhost:7258/api'; // Reemplacen con la de su computadora

// Manejar el inicio de sesión

const formLogin = document.getElementById('form-login');
formLogin.addEventListener('submit', async (e) => {
    e.preventDefault();

    const email = document.getElementById('correo-login').value;
    const password = document.getElementById('contrasena-login').value;

    if (!email || !password) {
        alert('Por favor, complete todos los campos.');
        return;
    }

    const envio = {

        correoUsu: email,
        contrasena: password //Cambiar esto en el backend
    }

    try {
        const response = await fetch(`${API_URL}/Login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(envio)
        });
        const data = await response.json();

        if (response.ok) {
            GuardarToken(data.token, data.usuario);
            window.location.href = '../pages/dashboard.html'; // Redirigir al dashboard que es nuestra página principal después del login
        }
        else { {
            alert(data.mensaje || 'Correo o contraseña incorrectos.');
        }}
    } catch (error) {
        console.error('Error durante el inicio de sesión:', error);
        alert('Ocurrió un error. Por favor, intente nuevamente.');
    }

});

// Manejar el registro

const formRegistro = document.getElementById('form-reg');
formRegistro.addEventListener('submit', async (e) => {
    e.preventDefault();
    const usuario = document.getElementById('usuario-reg').value;
    const email = document.getElementById('correo-reg').value;
    const password = document.getElementById('contrasena-reg').value;
    

    if (!usuario || !email || !password ) {
        alert('Por favor, complete todos los campos.');
        return;
    }

    if(!email.includes("@")) {
        alert('El correo debe tener un @.');
        return;
    }


    if(password.length < 12) { 
        alert('La contraseña debe tener al menos 12 caracteres.');
        return;
    }

    if(!/[A-Z]/.test(password)) {
        alert('La contraseña debe tener al menos una mayúscula.')
        return;
    }

    if(!/[0-9]/.test(password)) {
        alert('La contraseña debe tener al menos un número.')
        return;
    }

     const envio = {
        nombreUsu: usuario,
        correoUsu: email,
        contrasena: password //Cambiar esto en el backend
    }
    
    try {
        const response = await fetch(`${API_URL}/Registro/RegistrarUsuario`, { //Arreglar esta ruta en C#
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(envio)
        });
        const data = await response.json();
        if (response.ok) {
            alert('Registro exitoso. Ahora puedes iniciar sesión.');
            formRegistro.reset();
            document.getElementById('mostrar-login').click(); // Muestra el formulario de login
        } else {
            alert(data.mensaje || 'Error en el registro. Por favor, intente nuevamente.');
        }
    } catch (error) {
        console.error('Error durante el registro:', error);
        alert('Ocurrió un error. Por favor, intente nuevamente.');
    }
});


// Redirigir al usuario si ya está logueado
window.addEventListener('DOMContentLoaded', () => {
    if (estaLogueado()) {
        // Si ya tiene token, redirigir a dashboard
        window.location.href = '../pages/dashboard.html';
    }
});


