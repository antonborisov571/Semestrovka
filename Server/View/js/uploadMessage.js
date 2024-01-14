const fileInput = document.getElementById('formFileLg');

$(document).ready(function () {
    $("#submitButton").click(function (event) {
        const textMessage = $("#textMessage").val();
        let messageId = 0;
        $.ajax({
            url: window.location.href,
            type: 'POST',
            dataType: 'text',
            data: { textMessage: textMessage },
            success: function (result) {
                messageId = parseInt(result);
            },
        });

        for (let i = 0; i < fileInput.files.length; i++) {
            const file = fileInput.files[i];
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
                            url: `/addimagemessage/${messageId}`,
                            type: 'POST',
                            dataType: 'html',
                            data: { content: data.data.url },
                            success: function (data) {

                            }
                        });
                    },
                    error: function (error) {
                        console.error(error);
                    }
                });
            }
        }
        setTimeout(() => { location.reload(); }, 10000);
    });
});