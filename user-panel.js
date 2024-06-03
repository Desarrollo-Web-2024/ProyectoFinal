let activeEventsList = document.getElementById("active-events")
let pastEventsList = document.getElementById("past-events")
let submitEventButton = document.getElementById("create-event")
var user = JSON.parse(localStorage.getItem('user') || null)


const retrieveActiveEvents = async () => {
    try {
        const response = await fetch(`http://localhost:5286/event/get-unsolved?userId=${user.id}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            }
        })

        if(response.ok){
            const res = await response.json()
            console.log(res)
            activeEventsList.innerHTML = res.reduce((acc, e) => acc + `<li className="list-group-item">Solicitud ${e.name}</li>`, '')
        }else{
        }

    }catch (e) {
        console.log(e)
    }
}

const retrievePastEvents = async () => {
    try {
        const response = await fetch(`http://localhost:5286/event/get-solved?userId=${user.id}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            }
        })

        if(response.ok){
            const res = await response.json()
            console.log(res)
            pastEventsList.innerHTML = res.reduce((acc, e) => acc + `<li className="list-group-item">Solicitud ${e.name}</li>`, '')
        }else{
        }

    }catch (e) {
        console.log(e)
    }
}

submitEventButton.onclick = async () => {
    const randomName = window.crypto.randomUUID()
    const newEvent = {
        "name": randomName,
        "description": "A new event",
        "startDate": new Date(),
        "duration": 10,
        "clientId": user.id
    }

    const response = await fetch(`http://localhost:5286/event`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(newEvent)
    })
    await response.json()
    location.reload()

}

retrieveActiveEvents()
retrievePastEvents()
