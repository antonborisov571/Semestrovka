function getContent() {
    $.ajax({
        url: '/contentmain',
        method: 'get',
        dataType: 'html',
        data: { },
        success: function (result) {
            $('#content_placer').append(result)
        }
        });
}

$(document).ready(function () {
    getContent();
})