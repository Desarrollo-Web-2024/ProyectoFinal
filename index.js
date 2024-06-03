var navAccess = document.getElementById("nav-access")
var user = JSON.parse(localStorage.getItem('user') || null)
const logout = () => {
    localStorage.removeItem("user")
    location.reload()
}

if(user !== null) {
    var panel = user.type === 1 ? `
        <li class="nav-item">
            <a class="nav-link" href="user-panel.html">Panel Usuario</a>
        </li>
    ` : `
        <li class="nav-item">
            <a class="nav-link" href="admin-panel.html">Panel Admin</a>
        </li>
    `

    navAccess.innerHTML = `
        ${panel}
        <li class="nav-item">
            <button class="nav-link" id="logout">Cerrar Sesion</button>
        </li>
    `
    var logoutButton = document.getElementById("logout")
    logoutButton.onclick = logout
}else{
    navAccess.innerHTML = `
    <li class="nav-item">
        <a class="nav-link" href="signup.html">Registrarse</a>
    </li>
    <li class="nav-item">
        <a class="nav-link" href="login.html">Iniciar Sesión</a>
    </li>
    `
}