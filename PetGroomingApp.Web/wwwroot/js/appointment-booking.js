$(document).ready(function () {
    // Helper: restrict dates to working days/hours (Mon-Fri, 9am–6pm)
    $('#appointmentDateTime').on('change', function () {
        var dateTime = $(this).val();
        var date = new Date(dateTime);
        var day = date.getDay();
        var hour = date.getHours();

        // Only allow Mon-Fri, 9am-18pm
        if (day === 0 || day === 6 || hour < 9 || hour > 18) {
            alert('Please select a working day (Mon-Fri) and working hours (9:00-18:00).');
            $(this).val('');
            $('#groomerDropdown').empty().append('<option value="">Select a groomer</option>');
            return;
        }

        // Load available groomers for this time
        $.get('/api/groomers/available', { dateTime: dateTime }, function (groomers) {
            var dropdown = $('#groomerDropdown');
            dropdown.empty().append('<option value="">Select a groomer</option>');
            $.each(groomers, function (i, groomer) {
                dropdown.append(`<option value="${groomer.id}">${groomer.firstName} ${groomer.lastName}</option>`);
            });
        });
    });

    // When groomer changes, load their available times (optional calendar logic)
    $('#groomerDropdown').on('change', function () {
        var groomerId = $(this).val();
        if (groomerId) {
            $.get(`/api/groomers/${groomerId}/available-times`, function (times) {
                // Optionally show times in a calendar or dropdown.
                // You could populate #appointmentDateTime with allowed times.
                // For simple implementation, just alert available times:
                if (times && times.length > 0) {
                    alert('Available times for selected groomer:\n' + times.map(t => new Date(t).toLocaleString()).join('\n'));
                } else {
                    alert('No available times for this groomer.');
                }
            });
        }
    });

    // When services checkboxes change, get total duration & price
    $('#servicesCheckboxes').on('change', '.service-checkbox', function () {
        var selectedIds = [];
        var durations = [];
        var prices = [];
        $('.service-checkbox:checked').each(function () {
            selectedIds.push($(this).val());
            durations.push(parseInt($(this).data('duration')));
            prices.push(parseFloat($(this).data('price')));
        });

        if (selectedIds.length === 0) {
            $('#totalDuration').text('0');
            $('#totalPrice').text('0.00');
            return;
        }

        
        $.ajax({
            url: '/api/services/sum',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(selectedIds),
            success: function (result) {
                $('#totalDuration').text(result.totalDuration);
                $('#totalPrice').text(result.totalPrice.toFixed(2));
            }
        });
    });

    // Trigger totals calculation on page load if preselected
    $('.service-checkbox:checked').trigger('change');
});