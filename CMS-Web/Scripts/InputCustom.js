/* CKEDITOR */
function init_Ckeditor() {
    $('.ckeditor').ckeditor();
};
/* END CKEDITOR */

/* SWITCHERY */
function init_Switch() {
    if ($(".js-switch")[0]) {
        var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
        elems.forEach(function (html) {
            var switchery = new Switchery(html, {
                color: '#26B99A'
            });
        });
    }
    if ($(".js-switch-chk")[0]) {
        var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch-chk'));
        elems.forEach(function (html) {
            if (html.style.display != "none") {
                var switchery = new Switchery(html, {
                    color: '#26B99A'
                });
            }
        });
    }
};
/* END SWITCHERY */

/* ICHECK */
function init_ICheck() {
    if ($("input.icheck")[0]) {
        $(document).ready(function () {
            $('input.icheck').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_flat-green'
            });
        });
    }
};
/* END ICHECK */

function init_ICheckMoney() {
    //keyMoney
    $(".keyMoney").keypress(function (e) {
        if (window.event) {
            if (window.event.keyCode != 46 && window.event.keyCode > 31
                && (window.event.keyCode < 48 || window.event.keyCode > 57)) {
                return (false);//chrome and IE
            }
        } else {
            if (e.which != 46 && e.which > 31
                && (e.which < 48 || e.which > 57)) {
                return (false);//firefox
            }
        }
    });
}

$(document).ready(function () {
    init_Switch();
    init_ICheck();
    init_Ckeditor();
    init_ICheckMoney();
});