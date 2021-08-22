$(document).ready(function () {

    $("#AddUserToBoardBtn").click(function () {
        const userid = $("#KullaniciID").val();
        const boardid = $("#PanoID").val();

        $.ajax({
            type: "POST",
            async: true,
            url: "/Board/AddUserToBoard",
            data: { userid, boardid },
            dataType: "text",
            success: function (msg) {
                if (msg === "true")
                    location.reload();
            }

        });

    })


    $(".removeUserFromBoardBtn").click(function () {
        const userid = $(this).attr('id');
        const boardid = $("#PanoID").val();

        $.ajax({
            type: "POST",
            async: true,
            url: "/Board/RemoveUserFromBoard",
            data: { userid, boardid },
            dataType: "text",
            success: function (msg) {
                if (msg === "true")
                    location.reload();
            }

        });
    })


    $(".add-user-to-board-btn").click(function () {
        let isShow = $("#addUserToBoard").attr('show');
        if (isShow == "1") {
            $("#addUserToBoard").attr('show', '0');
            $("#addUserToBoard").hide(600);
            $("#addUserToBoard").addClass('hidden');
        }
        else {
            $("#addUserToBoard").attr('show', '1');
            $("#addUserToBoard").show(600);
            $("#addUserToBoard").removeClass('hidden');
        }
    })



})