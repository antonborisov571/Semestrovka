const fileInput = document.getElementById('formFileLg');

$(document).ready(function () {
    $("#submitButton").click(function (event) {
        const file = fileInput.files[0];
        if (file) {
            const formData = new FormData();
            formData.append('image', file);
            $.ajax({
                url: 'https://api.imgbb.com/1/upload?expiration=15552000&key=2113c7a32f2b2ee90bcec38bd2cbe87a',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    $.ajax({
                        url: '/addimageprofile',
                        type: 'POST',
                        dataType: 'html',
                        data: { content: data.data.url }
                    });
                    window.location.replace("/profile");
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
    });
});