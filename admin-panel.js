let activeEventsList = document.getElementById("active-events")
let activeTimeEventList = document.getElementById("active-events-time")
let submitEventButton = document.getElementById("create-event")
var user = JSON.parse(localStorage.getItem('user') || null)


const retrieveActiveEvents = async () => {
    try {
        const response = await fetch(`http://localhost:5286/event/get-unsolved-all`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            }
        })

        if(response.ok){
            const res = await response.json()
            console.log(res)
            activeEventsList.innerHTML = res.reduce((acc, e) => acc + `<li className="list-group-item">Solicitud ${e.name} Id: ${e.id}</li>`, '')
            activeTimeEventList.innerHTML = res.reduce((acc, e) => {
                const diffTime = Math.abs(new Date() - new Date(e.startDate));
                const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));
                return acc + `<li className="list-group-item">Solicitud Id: ${e.id} Tiempo Activo: ${diffDays} dias</li>`
            }, '')
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
