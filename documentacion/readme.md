## 📚 Estructura Detallada

### 🖥️ **Frontend**

#### **assets/**
Contiene imágenes, íconos y fuentes usadas en la interfaz.

#### **css/**
- `styles.css`: estilos globales  
- `variables.css`: colores, espaciados y tipografías  
- `components/`: estilos específicos para componentes (navbar, cards, forms…)  
- `pages/`: estilos personalizados por cada página  

#### **js/**
- `main.js`: punto de entrada principal  
- `config.js`: URLs del backend, variables globales  
- `utils/`: helpers, validadores y constantes  
- `services/`: comunicación con la API (auth, CRUD, etc.)  
- `components/`: scripts de modales, dropdowns y UI reutilizable  
- `pages/`: lógica para cada página

#### **pages/**
HTML de cada vista de la app:
- `index.html`
- `dashboard.html`
- `login.html`

---

### 🔧 **Backend – C#**
Ubicado en la carpeta `/backend`.

- **Controllers/** → Endpoints de la API  
- **Models/** → Tablas y entidades  
- **Services/** → Lógica de negocio  
- **DTOs/** → Transporte seguro de datos  
- **Data/** → DbContext  
- **Middleware/** → JWT, validaciones, manejo de errores  
- **MyProject.Tests/** → Pruebas automatizadas  

---

### 🗄️ **Database/**
- **migrations/** → Scripts de EF Core  
- **seeds/** → Datos iniciales (usuarios, categorías, etc.)

---

### 📘 **Docs/**
Documentación adicional:
- `api-docs.md`: rutas, parámetros y respuestas de la API

---

## ⚙️ Instalación y Configuración

### 1️⃣ Clonar repositorio
```bash
git clone https://github.com/tu-usuario/mi-proyecto.git