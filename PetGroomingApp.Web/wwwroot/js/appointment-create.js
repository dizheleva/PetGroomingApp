document.addEventListener("DOMContentLoaded", function () {
    const serviceSelect = document.getElementById("service-select");
    const timeInput = document.getElementById("appointment-time");
    const groomerSelect = document.getElementById("groomer-select");

    // ---- SERVICES CHANGE HANDLER ----
    serviceSelect.addEventListener("change", async () => {
        const selectedIds = [...serviceSelect.selectedOptions].map(o => o.value);
        if (selectedIds.length === 0) return;

        const response = await fetch("/api/AppointmentApi/CalculateTotals", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            credentials: 'include',
            body: JSON.stringify({ selectedServiceIds: selectedIds })  // ✅ FIXED HERE
        });

        if (response.ok) {
            const data = await response.json();
            document.getElementById("duration-output").textContent = data.totalDuration;
            document.getElementById("price-output").textContent = data.totalPrice.toFixed(2);
        } else {
            console.error("Failed to calculate totals", response.status);
        }
    });

    // ---- TIME CHANGE HANDLER ----
    timeInput.addEventListener("change", async () => {
        const appointmentTime = timeInput.value;
        if (!appointmentTime) return;

        const response = await fetch("/api/AppointmentApi/GetAvailableGroomers", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            credentials: 'include',
            body: JSON.stringify({ time: appointmentTime })  
        });

        if (response.ok) {
            const groomers = await response.json();
            groomerSelect.innerHTML = "";

            groomers.forEach(g => {
                const opt = document.createElement("option");
                opt.value = g.id;
                opt.text = g.name;
                groomerSelect.appendChild(opt);
            });
        } else {
            console.error("Failed to load groomers", response.status);
        }
    });
});
