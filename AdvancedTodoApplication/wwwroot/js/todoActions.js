$(document).ready(function () {

    $(".add-user-to-todo-item").click(function () {
        const todoid = $(this).attr('todoid');
        const userid = $(this).attr('userid');
        const boardid = $(this).attr('boardid');

        $.ajax({
            type: "POST",
            async: true,
            url: "/ToDo/AddUserToToDo",
            data: { userid, todoid, boardid },
            dataType: "text",
            success: function (msg) {
                if (msg === "true")
                    location.reload();
            }
        });
    })


    $(".todo-checkbox").click(function () {
        const todoid = $(this).attr('todoid');
        const isChecked = $(this).attr('ischecked');

        $.ajax({
            type: "POST",
            async: true,
            url: "/ToDo/CheckThisToDo",
            data: { todoid },
            dataType: "text",
            success: function (msg) {
                if (msg === "true")
                    location.reload();
                //if (isChecked === "1") {
                //    // yapýlmadý olarak ayarla
                //    $(this).attr("ischecked", "0");
                //    $(this).removeAttr("checked");
                //}
                //else {
                //    $(this).attr("ischecked", "1");
                //    $(this).attr("checked", "1");

                //    // yapýldý olarak ayarla
                //}

            }
        });
    })


    $(".remove-user-from-todo").click(function () {
        const userid = $(this).attr('userid');
        const todoid = $(this).attr('todoid');

        $.ajax({
            type: "POST",
            async: true,
            url: "/ToDo/RemoveUserFromToDo",
            data: { userid, todoid },
            dataType: "text",
            success: function (msg) {
                if (msg === "true")
                    location.reload();
            }

        });
    })



})