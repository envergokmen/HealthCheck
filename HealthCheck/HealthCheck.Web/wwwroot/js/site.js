// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {


    if ($("#AppStatuses").length > 0) {

        setInterval(function () {

            $.ajax('/AppManager/GetStatuses',
                {
                    cache: false,
                    success: function (data) {

                        $newTable = $(data);

                        $("tbody tr", $newTable).each(function () {

                            var id = $(this).attr("id");

                            //there is a new element
                            if ($(`#${id}`).length <= 0) {

                                $elem = $(this);
                                $elem.addClass("new");
                                $("#AppStatuses table tbody").prepend($elem);

                                setTimeout(function () { $elem.removeClass("new"); }, 2000);
                            }

                            //contents are different
                            if ($(this).text() != $(`#${id}`).text()) {

                                $(`#${id}`).addClass("new");
                                setTimeout(function () { $(`#${id}`).removeClass("new"); }, 2000);

                                $(`#${id}`).html($(this).html());

                            }

                        });
                    }
                });

        }, 3000);

        var itemToDeleteId = 0;

        $("#AppStatuses table").on("click", ".delete", function (e) {

            e.preventDefault();

            itemToDeleteId = $(this).attr("id");
            $("#itemname").text($(this).attr("name"));

            $('#Confirm').modal('show');
        });


        $("#Confirm").on("click", "#deleteConfirm", function (e) {
            $deletedElem = $("#tr-"+ itemToDeleteId);

            $.ajax(`/AppManager/Delete/${itemToDeleteId}`,
                {
                    success: function (data) {

                        if (data == "true") {

                            $('#Confirm').modal('hide');

                            $deletedElem.addClass("deleted");
                            $deletedElem.fadeOut("slow");

                        } else {
                            alert("error on delete");
                        }
             
                    }
                });
        });
    }


});