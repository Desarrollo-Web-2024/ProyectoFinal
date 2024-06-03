let newUserForm = document.getElementById("new-user-form")
let nameInput = document.getElementById("nombre")
let lastNameInput = document.getElementById("apellido")
let emailInput = document.getElementById("email")
let passwordInput = document.getElementById("password")

newUserForm.onsubmit = async (e) => {
    e.preventDefault()

    let newUser = {
        id: 0,
        username: emailInput.value,
        name: nameInput.value,
        lastName: lastNameInput.value,
        passwordHash: passwordInput.value,
        type: 1
    }

    try {
        console.log(newUser)
        const response = await fetch("http://localhost:5286/user", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(newUser)
        })

        if(response.ok){
            const res = await response.json()
            window.location.href = "./index.html"
        }

    }catch (e) {
        console.log(e)
    }

}