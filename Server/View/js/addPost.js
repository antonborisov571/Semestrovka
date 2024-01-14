function addFavourite(value) {
    let messageId = $(value).attr('messageid');
    let hasFavourite = $(value).attr('hasfavourite');
    if (hasFavourite === "True") {
        value.setAttribute('hasfavourite', "False");
        value.style = "color:gray;";
    } else {
        value.setAttribute('hasfavourite', "True");
        value.style = "color:orange;";
    }
    $.ajax({
        url: '/addfavourite',
        method: 'post',
        dataType: 'html',
        data: { content: messageId }
    });
}

function setLike(value) {
    let messageId = $(value).attr('messageid');
    let hasLike = $(value).attr('haslike');
    if (hasLike === "True") {
        value.setAttribute('haslike', "False");
        value.style = "color:gray;";
    } else {
        value.setAttribute('haslike', "True");
        value.style = "color:red;";
    }
    $.ajax({
        url: '/setlike',
        method: 'post',
        dataType: 'html',
        data: { content: messageId }
    });
}