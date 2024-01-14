document.getElementById("password").addEventListener("keyup", (event) => {
    var isAnyInvalid = false;
    var inputParameters = [
        {
            param: "letter",
            regex: /[a-z]/g
        },
        {
            param: "capital",
            regex: /[A-Z]/g
        },
        {
            param: "symbol",
            regex: /[,\.?<>\\!@#$%^&*()_\+\-=/~]/g
        },
        {
            param: "number",
            regex: /[0-9]/g
        },
        {
            param: "length",
            regex: /.{8,}/g
        }
    ];

    for (let i = 0; i < inputParameters.length; i++)
    {
        if (!event.target.value.match(inputParameters[i].regex)) {
            switch (inputParameters[i].param){
                case 'letter':
                    document.getElementById("labelPass").innerText = "Пароль должен содержать хотя бы одну строчную английскую букву";
                    break;
                case 'capital':
                    document.getElementById("labelPass").innerText = "Пароль должен содержать хотя бы одну строчную английскую букву";
                    break;
                case 'symbol':
                    document.getElementById("labelPass").innerText = "Пароль должен содержать хотя бы один спец. символ: ,.?<>\\!@#$%^&*()_+-=/~";
                    break;
                case 'number':
                    document.getElementById("labelPass").innerText = "Пароль должен содержать хотя бы одну цифру";
                    break;
                case 'length':
                    document.getElementById("labelPass").innerText = "Пароль должен содержать хотя бы 8 символов";
                    break;
                default:
                    document.getElementById("labelPass").innerText = "Пароль содержит недопустимый символ";
            
            }
            isAnyInvalid = true;
        }
    }

    if (!isAnyInvalid){
        document.getElementById("labelPass").innerText = "";
    }
});

document.getElementById("login").addEventListener("keyup", (event) => {
    var isAnyInvalid = false;
    var inputParameters = [
        {
            param: "symbol",
            regex: /^[a-zA-Z_]([a-zA-Z0-9\-_\.])*$/g
        }
    ];

    for (let i = 0; i < inputParameters.length; i++)
    {
        if (!event.target.value.match(inputParameters[i].regex)) {
            isAnyInvalid = true;
        }
    }

    if (isAnyInvalid) {
        document.getElementById("labelLogin").innerText = "Неверно введён логин";
    } else {
        document.getElementById("labelLogin").innerText = "";
    }
    
});

document.getElementById("email").addEventListener("keyup", (event) => {
    var isAnyInvalid = false;
    var inputParameters = [
        {
            param: "symbol",
            regex: /[@]/g
        }
    ];

    for (let i = 0; i < inputParameters.length; i++)
    {
        if (!event.target.value.match(inputParameters[i].regex)) {
            isAnyInvalid = true;
        }
    }

    if (isAnyInvalid) {
        document.getElementById("labelEmail").innerText = "Неверно введена электронная почта";
    } else {
        document.getElementById("labelEmail").innerText = "";
    }
    
});