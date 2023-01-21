$(document).ready(function () {
    $('.menu li div').hide();
    $('#expand_menu_container li div').hide();
})

function openSubmenu(val) {
    var id = val.split('-')[1];
    if ($('#div-' + id).is(':visible')==false) {
        $('.menu li div').slideUp();  
    }
    else{
        $('#div-' + id).slideUp();
        return
    }
    $('#div-' + id).slideDown('fast')
}

function openMenuExpand(val){
    var id= val.split('-')[1];
    if ($('#div_expand-' + id).is(':visible')==false) {
        $('#menuExpand li div').slideUp();
    }
    else{
        $('#div_expand-' + id).slideUp();
        return
    }
    $('#div_expand-' + id).slideDown('fast')
}

$(document).mouseup(function(e) 
{
    var container = $("#menuExpand li div");

    // if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) 
    {
        container.slideUp('fast');
    }
});
function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}


$(document).ready(function () {

    $('.event_detail').each(function () {
        var val = $(this).html();
        $(this).html($(this).html().replace(/(?:\r\n|\r|\n)/g, '<br>'))
    })

   
})

