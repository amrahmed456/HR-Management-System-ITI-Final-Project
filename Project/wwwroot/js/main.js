$(document).ready(function () {

    $("button[type='submit']").on("click", function () {
        $("button .indicator-label").addClass("d-none");
        $("button .indicator-progress").addClass("d-block");
    });
    if ($("#run-success-mssg").length > 0) {
        Swal.fire({
            title: 'Success!',
            text: 'Executed Successfully',
            icon: 'success',
            confirmButtonText: 'ok'
        })
    }
    if ($("#run-faild-mssg").length > 0) {
        let message = ($("#run-faild-mssg").text().length > 5) ? $("#run-faild-mssg").text() : 'There was unknown error, Please try again.';
        Swal.fire({
            title: 'Error!',
            text: message,
            icon: 'error',
            confirmButtonText: 'ok'
        })
    }

    $(".custom_date_picker").flatpickr();

    $(".custom_time_picker").flatpickr({
        enableTime: true,
        noCalendar: true,
        dateFormat: "H:i",
    });


    $(".custom_date_range_picker").daterangepicker();

    $("[data-kt-customer-table-filter='search']").quicksearch('table tbody tr', {});



    // load employees for new attendance modal
    var newAttendanceModalLoaded = false;
    $("[data-bs-target='#kt_create_new_attendance']").on("click", function () {
        if (!newAttendanceModalLoaded) {
            // load employees list
            $.ajax({
                url: "/attendance/getEmployees",
                method: "POST",
                dataType: "text",
                success: function (response) {
                    newAttendanceModalLoaded = true;
                    var emps = JSON.parse(response);
                    for (var i = 0; i < emps.length; i++) {
                        var emp = emps[i];
                        $("#emps_attendance_selectbox").append(`<option value="${emp.ssn}">${emp.name} - (${emp.departmentName})</option>`);
                    }
                    if (response.length < 5) {
                        $(".modal-create-attendance").html(`<div class="alert alert-warning">All Employees Took Attendance Of Today</div>`);
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            })
        }
    });


    ////$("[data-bs-target='#kt_modal_add_customer']").on("click", function () {
    ////});
    //$(document).ready(function () {
    //    $("#addEmpModalBtn").on("click", function () {
    //        $.ajax({
    //            url: "/Department/getDepartments",
    //            method: "POST",
    //            dataType: "text",
    //            success: function (response) {
    //                var departmentsList = JSON.parse(response);
    //                for (var i = 0; i < departmentsList.length; i++) {
    //                    var dep = departmentsList[i];
    //                    $("#addEmpDeptSelect").append(`<option value="${dep.Id}">${dep.Name}</option>`);
    //                }
    //            },
    //            error: function (err) {
    //                console.log(err);
    //            }
    //        });
    //    });
    //});









    $(".applyBtn").on("click", function () {
        setTimeout(function () {
            $("form#filter-form").submit();
        }, 400);
    })
})