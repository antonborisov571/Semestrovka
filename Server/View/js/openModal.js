function deleteMessage(value) {
    let messageId = $(value).attr('messageid');
    $.ajax({
        url: '/deletemessage',
        method: 'post',
        dataType: 'html',
        data: { content: messageId }
    });
}

function openModal(value) {
  document.querySelector(`#modal_${$(value).attr('messageid')}`).style.display = 'flex';
};

function no(value) {
  document.querySelector(`#modal_${$(value).attr('messageid')}`).style.display = 'none';
};

function yes(value) {
  document.querySelector(`#modal_${$(value).attr('messageid')}`).style.display = 'none';
    deleteMessage(value);
    setTimeout(() => { location.reload(); }, 250);
    
};

