function LoadGrid() {
    var form = $(".search-form");
    //console.log($(form).attr("action"))
    $.ajax({
        url: $(form).attr('action'),
        type: "GET",
        data: $(form).serialize(),
        dataType: 'html',
        beforeSend: function () {
            $('.se-pre-con').show();
        },
        //processData: false,
        //contentType: false,
        success: function (data) {
            $(".detail-view").html(data);
        },
        complete: function () {
            $('.se-pre-con').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('.se-pre-con').hide();
        }
    });
}

function PreviewImage(e, previewElementID) {
    var oFReader = new FileReader();
    oFReader.readAsDataURL(e.files[0]);

    oFReader.onload = function (oFREvent) {
        document.getElementById(previewElementID).src = oFREvent.target.result;
    };
};

function ShowViewOrEdit(action) {
    $.ajax({
        url: action,
        beforeSend: function () {
            $(".se-pre-con").show();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
            $(".se-pre-con").hide();
        },
        complete: function () {
            $(".se-pre-con").hide();
        },
        success: function (html) {
            
            ShowDetail(html);
        }
    });
}

function ShowDetail(content) {
    $(".detail-view").html(content);
    $(".detail-view").show();
    $("._gridview").css("display", "none");

}

function CloseDetail() {
    LoadGrid();
}



/* ICHECK */
function init_ICheck() {
    if ($("input.icheck")[0]) {
        $(document).ready(function () {
            $('input.icheck').iCheck({
                checkboxClass: 'icheckbox_square-grey',
                radioClass: 'iradio_square-green'
            });
        });
    }
};
/* END ICHECK */



function SubmitForm(action)
{
    $(action).submit();
}

$(document).ready(function () {
    init_ICheck();
});