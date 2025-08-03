async function fetchTotals() {
    const selectedServices = [...document.getElementById("service-select").selectedOptions].map(o => o.value);

    const response = await fetch("/api/AppointmentApi/CalculateTotal", {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(selectedServices)
    });

    if (response.ok) {
        const data = await response.json();
        document.getElementById("total-duration").innerText = data.totalDuration;
        document.getElementById("total-price").innerText = data.totalPrice.toFixed(2);
    }
}

async function fetchAvailableGroomers() {
    const time = document.getElementById("appointment-time").value;
    const response = await fetch("/api/AppointmentApi/CheckAvailableGroomers", {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(time)
    });

    if (response.ok) {
        const groomers = await response.json();
        const groomerSelect = document.getElementById("groomer-select");
        groomerSelect.innerHTML = "";
        groomers.forEach(g => {
            const option = document.createElement("option");
            option.value = g.id;
            option.text = g.name;
            groomerSelect.appendChild(option);
        });
    }
}

document.getElementById("service-select").addEventListener("change", fetchTotals);
document.getElementById("appointment-time").addEventListener("change", fetchAvailableGroomers);