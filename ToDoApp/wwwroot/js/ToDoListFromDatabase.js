$(document).ready(function () {
    editModal();
    modalUpdate();
});

function editModal() {
    $("#Edit").on("click", function () {
        var id = $(this).attr("data-id");
        console.log(id);
        $.ajax({
            type: 'GET',
            url: '/Todos/Edit/' + id,
            success: function (task) {
                $('#task').val(task.task);
                $('#deadline').val(task.deadline);
                $('#notes').val(task.notes);
            }
        });
    });
}
function modalUpdate() {
    $("button[data-save='modal']").on("click", function () {
        var form = $('form');
        var toke = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            type: 'POST',
            url: '/Todos/EditTodo/' + data,
            data: {
                __RequestVerificationToken: token,
                position: {
                    PositionName: $("#PositionName").val()
                }
            },
            success: function (data) {
                if (data.success == true) {
                    console.log("lähetetty data: " + data);
                    $('#Edit').modal('hide');
                    location.reload(false)
                } else {
                    console.log("failed data: " + data);
                    $('#Edit').html(data);
                }
            }
        })
    });
}

function loadData() {
    $.ajax({
        type: 'GET',
        url: 'home/getByUser',
        dataType: 'json',
        success: function (data) {
            data.data.sort(function (a, b) {
                return new Date(b.deadline) - new Date(a.deadline);
            });
            $.each(data.data, function (i, item) {
                var falseDate = new Date(2000, 1, 1);
                var deadlineParsed = new Date(item.deadline);
                var deadlineFormatted = deadlineParsed.getDate() + "." + (deadlineParsed.getMonth() + 1) + "." + deadlineParsed.getFullYear() + " " + deadlineParsed.getHours() + ":" + deadlineParsed.getMinutes();
                if (item.isDone === false) {
                    if (falseDate.getFullYear() > deadlineParsed.getFullYear()) {
                        console.log(falseDate.getFullYear()+ " " + deadlineParsed.getFullYear());
                        $("#endlessTasklist").append(`<div class="col-11 m-0 p-0">
                                            <div class="alert alert-success">${item.task}</div>
                                        </div>
                                        <div class="col-1 m-0 p-0">
                                            <div class="alert alert-success" id="done" onclick=Update('/home/Update?id='+${item.id})><i class="fas fa-check"></i></div>
                                        </div>`);
                    }
                    else {
                        $("#unfinishedTasklist").append(`<div class="col-11 m-0 p-0">
                                            <div class="alert alert-primary">${item.task} ${deadlineFormatted}</div>
                                        </div>
                                        <div class="col-1 m-0 p-0">
                                            <div class="alert alert-success" id="done" onclick=Update('/home/Update?id='+${item.id})><i class="fas fa-check"></i></div>
                                        </div>`);
                    }

                } else {
                    $("#finishedTasklist").append(`<div class="col-11 m-0 p-0">
                                            <div class="alert alert-secondary"><del>${item.task} ${deadlineFormatted}</del></div>
                                        </div>
                                        <div class="col-1 m-0 p-0">
                                            <div class="alert alert-secondary" id="notDone"><i class="fas fa-check"></i></div>
                                        </div>`);
                }
                
            });
        }
    });
}


function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);   
                        location.reload();
                    }
                    else {
                        toastr.error(data.message)
                    }

                }
            });
        }
    });
}
