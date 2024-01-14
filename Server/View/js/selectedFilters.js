
let selectedType = $('.dropdown-menu a[data-type]').data('type');
let selectedTime = $('.dropdown-menu a[data-time]').data('time');

$(document).ready(function () {
    $('.dropdown-menu a[data-type]').on('click', function (e) {
        e.preventDefault();
        selectedType = $(this).data('type');
        $('.dropdown-menu a[data-type]').removeClass('active');
        $(this).addClass('active');
    });

    $('.dropdown-menu a[data-time]').on('click', function (e) {
        e.preventDefault();
        selectedTime = $(this).data('time');
        $('.dropdown-menu a[data-time]').removeClass('active');
        $(this).addClass('active');
    });
});


$(document).ready(function () {
    $('#submitButton').click(function (event) {
        const text = $("#textFind").val();
        window.location.href = `/find/${selectedType}/${selectedTime}?textFind=${encodeURIComponent(text)}`

        /*$.ajax({
            url: `/find/${selectedType}/${selectedTime}`,
            method: 'get',
            dataType: 'html',
            data: { textFind: text },
        });*/
    });
}); 