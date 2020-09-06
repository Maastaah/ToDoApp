$(document).ready(function () {
    loadData();
 
    //loadDataTable();
});

function loadData() {
    $.ajax({
        type: 'GET',
        url: 'home/getall',
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

function Update(url) {
    console.log(url);
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willUpdate) => {
        console.log(url);
        $.ajax({
            type: "POST",
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
    });
}