const form = document.getElementById("login-form");
const email = document.getElementById("email");
const password = document.getElementById("password");
const msg = document.getElementById("login-error")
form.addEventListener("submit", async(event) => {
    event.preventDefault()

    // Submit login request

    const loginRequest = {username: email.value, password: password.value}
    try {
        const response = await fetch("http://localhost:5286/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(loginRequest)
        })

        if(response.ok){
            const res = await response.json()
            localStorage.setItem('user', JSON.stringify(res))
            window.location.href = "./index.html"
        }else{
            msg.innerText = "Ocurrio un error al inciar sesión"
        }

    }catch (e) {
        console.log(e)
    }

})
