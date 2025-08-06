document.addEventListener("DOMContentLoaded", () => {
    const $ = (selector) => document.querySelector(selector);
    const $$ = (selector) => Array.from(document.querySelectorAll(selector));

    const appointmentTimeSelect = $('#AppointmentTime');
    const groomerSelect = $('#SelectedGroomerId');
    const serviceCheckboxes = $$('input[name="SelectedServiceIds"]');
    const durationDisplay = $('#durationValue');
    const priceDisplay = $('#priceValue');

    const baseUrl = 'https://localhost:7116/api';

    let selectedDuration = 0;

    // Format duration as HH:mm
    const formatMinutesToHHMM = (minutes) => {
        if (!minutes || isNaN(minutes)) return "00:00";
        const hours = Math.floor(minutes / 60);
        const mins = minutes % 60;
        return `${hours.toString().padStart(2, '0')}:${mins.toString().padStart(2, '0')}`;
    };

    // Update duration and price UI
    const updateTotals = (duration, price) => {
        durationDisplay.textContent = formatMinutesToHHMM(duration);
        priceDisplay.textContent = price.toFixed(2);
    };

    // Fetch totals (duration and price)
    const fetchTotals = async () => {
        const selectedIds = serviceCheckboxes
            .filter(cb => cb.checked)
            .map(cb => cb.value);

        if (selectedIds.length === 0) {
            updateTotals(0, 0);
            selectedDuration = 0;
            appointmentTimeSelect.disabled = true;
            appointmentTimeSelect.innerHTML = "<option value=''>Select time...</option>";
            return;
        }

        try {
            const res = await fetch(`${baseUrl}/AppointmentApi/CalculateTotals`, {
                method: 'POST',
                credentials: "include",
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ SelectedServiceIds: selectedIds })
            });

            if (!res.ok) {
                const errText = await res.text();
                console.error("❌ Server error:", errText);
                return;
            }

            const { totalDuration, totalPrice } = await res.json();
            selectedDuration = totalDuration || 0;
            updateTotals(totalDuration, totalPrice);
            fetchGroomers();

            if (groomerSelect.value) {
                fetchAvailableTimes(groomerSelect.value, selectedDuration);
            }

        } catch (err) {
            console.error("⚠️ Failed to calculate totals:", err);
        }
    };

    // Fetch available groomers based on time + duration
    const fetchGroomers = async () => {
        // For simplicity, we don't refetch groomers in this demo
        // You can add similar logic as before if groomer list is dynamic
    };

    // Fetch available time slots for selected groomer
    const fetchAvailableTimes = async (groomerId, duration) => {
        if (!groomerId || duration <= 0) {
            appointmentTimeSelect.disabled = true;
            appointmentTimeSelect.innerHTML = "<option value=''>Select time...</option>";
            return;
        }

        try {
            const res = await fetch(`${baseUrl}/GroomerApi/AvailableTimes`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    id: groomerId,
                    duration: duration
                })
            });

            if (!res.ok) {
                const err = await res.text();
                console.error("❌ Times fetch failed:", err);
                appointmentTimeSelect.disabled = true;
                appointmentTimeSelect.innerHTML = "<option value=''>No available slots</option>";
                return;
            }

            const times = await res.json(); // should be ISO strings or "yyyy-MM-ddTHH:mm"
            updateAppointmentTimeSelect(times);

        } catch (err) {
            console.error("⚠️ Fetch times error:", err);
            appointmentTimeSelect.disabled = true;
            appointmentTimeSelect.innerHTML = "<option value=''>No available slots</option>";
        }
    };

    // Populate the select with available times
    const updateAppointmentTimeSelect = (availableTimes) => {
        appointmentTimeSelect.innerHTML = "<option value=''>Select time...</option>";
        if (!availableTimes || availableTimes.length === 0) {
            appointmentTimeSelect.disabled = true;
            appointmentTimeSelect.innerHTML = "<option value=''>No available slots</option>";
            return;
        }

        availableTimes.forEach(dtIso => {
            const date = new Date(dtIso);
            const pad = n => n.toString().padStart(2, '0');
            const dateStr = `${pad(date.getDate())}-${pad(date.getMonth() + 1)}-${date.getFullYear()}`;
            const timeStr = `${pad(date.getHours())}:${pad(date.getMinutes())}`;
            appointmentTimeSelect.innerHTML += `<option value="${dtIso}">${dateStr} ${timeStr}</option>`;
        });
        appointmentTimeSelect.disabled = false;
    };

    // Event Bindings
    const bindEvents = () => {
        groomerSelect?.addEventListener('change', () => {
            if (groomerSelect.value && selectedDuration > 0) {
                fetchAvailableTimes(groomerSelect.value, selectedDuration);
            } else {
                appointmentTimeSelect.disabled = true;
                appointmentTimeSelect.innerHTML = "<option value=''>Select time...</option>";
            }
        });
        serviceCheckboxes.forEach(cb => cb.addEventListener('change', fetchTotals));
    };

    // Init
    const init = () => {
        if (!appointmentTimeSelect || !groomerSelect) {
            console.warn("Missing inputs in DOM.");
            return;
        }

        updateTotals(0, 0);
        bindEvents();
    };

    init();
    // Optionally, trigger an initial fetch for first-load state
    // fetchTotals();
});
//document.addEventListener("DOMContentLoaded", () => {
//    const $ = selector => document.querySelector(selector);
//    const $$ = selector => Array.from(document.querySelectorAll(selector));

//    const baseUrl = 'https://localhost:7116/api';

//    const serviceCheckboxes = $$('input[name="SelectedServiceIds"]');
//    const groomerSelect = $('#SelectedGroomerId');
//    const appointmentTimeInput = $('#AppointmentTime');
//    const durationDisplay = $('#durationValue');
//    const priceDisplay = $('#priceValue');

//    let selectedDuration = 0;

//    const pad = (n) => n.toString().padStart(2, '0');

//    const toDateTimeLocalString = (date) => {
//        if (!(date instanceof Date) || isNaN(date)) return '';
//        return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
//    };

//    const updateTotals = (duration, price) => {
//        const hours = Math.floor(duration / 60);
//        const minutes = duration % 60;
//        durationDisplay.textContent = `${pad(hours)}:${pad(minutes)}`;
//        priceDisplay.textContent = price.toFixed(2);
//    };

//    const fetchTotals = async () => {
//        const selectedIds = serviceCheckboxes.filter(cb => cb.checked).map(cb => cb.value);

//        if (selectedIds.length === 0) {
//            updateTotals(0, 0);
//            selectedDuration = 0;
//            groomerSelect.innerHTML = '<option value="">Select service first</option>';
//            appointmentTimeInput.value = '';
//            return;
//        }

//        const res = await fetch(`${baseUrl}/AppointmentApi/CalculateTotals`, {
//            method: 'POST',
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify({ SelectedServiceIds: selectedIds })
//        });

//        const { totalDuration, totalPrice } = await res.json();
//        selectedDuration = totalDuration;
//        updateTotals(totalDuration, totalPrice);

//        await fetchGroomers();
//        await fetchAvailableTimes();
//    };

//    const fetchGroomers = async () => {
//        if (selectedDuration === 0) return;

//        const res = await fetch(`${baseUrl}/GroomerApi/AvailableGroomers`, {
//            method: 'POST',
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify({
//                appointmentTime: new Date().toISOString(),
//                durationMinutes: selectedDuration
//            })
//        });

//        const groomers = await res.json();
//        groomerSelect.innerHTML = '<option value="">Any Available Groomer</option>';
//        groomers.forEach(g => {
//            const opt = new Option(g.name, g.id);
//            groomerSelect.add(opt);
//        });
//    };

//    const fetchAvailableTimes = async () => {
//        if (selectedDuration === 0) return;

//        const groomerId = groomerSelect.value;

//        const res = await fetch(`${baseUrl}/GroomerApi/AvailableTimes`, {
//            method: 'POST',
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify({
//                id: groomerId || '', // send empty string if not selected
//                duration: selectedDuration
//            })
//        });

//        const times = await res.json();
//        if (!Array.isArray(times) || times.length === 0) {
//            appointmentTimeInput.value = '';
//            return;
//        }

//        // Take first available time and set it to input
//        const firstSlot = new Date(times[0]);
//        appointmentTimeInput.value = toDateTimeLocalString(firstSlot);
//    };

//    serviceCheckboxes.forEach(cb => cb.addEventListener('change', fetchTotals));
//    groomerSelect.addEventListener('change', fetchAvailableTimes);
//});
