$(document).ready(function () {
    ShowEmployeeData();
});
function ShowEmployeeData() {
    $.ajax({
        url: '/Ajax/EmployeeList',
        type: 'Get',
        dataType: 'json',

        success: function (result, status, xhr) {
            var object = '';
            $.each(result, function (index, item) {
                object += '<tr>';
                object += '<td>' + item.employeeId + '</td>';
                object += '<td>' + item.firstName + '</td>';
                object += '<td>' + item.lastName + '</td>';
                object += '<td>' + item.email + '</td>';
                object += '<td>' + item.dateofBirth + '</td>';
                object += '<td>' + item.gender + '</td>';
                object += '<td>' + item.photoPath + '</td>';
                object += '<td><a class="btn btn-warning" onclick="Edit(' + item.employeeId + ')">Edit</a>  <a class="btn btn-danger" onclick="Delete(' + item.employeeId + ')">Delete</a></td>';
                object += '</tr>';

            });
            $('#tableData').html(object);
        },
        error: function () {
            alert("Error fetching data");
        }
    });
}
function ShowDepartmentData(id) {

    $.ajax({
        url: '/Ajax/DepartmentList',
        type: 'Get',
        dataType: 'json',

        success: function (result, status, xhr) {

            var object = '';
            object += '<option value="0">Select Department</option>'
            $.each(result, function (index, item) {
                if (item.departmentId == id) {
                    object += '<option value="' + item.departmentId + '" selected>' + item.departmentName + '</option>';
                }
                else {
                    object += '<option value="' + item.departmentId + '">' + item.departmentName + '</option>';
                }
                



            });
            $('#DepartmentId').html(object);
        },
        error: function () {
            alert("Error fetching data");
        }
    });
}
$('#addEmployee').click(function () {
    ResetModal();
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#empId').hide();
    $('#modalHeader').html('Add Employee');
    $('#employeeModal').modal('show');
    ShowDepartmentData(0);
    previewImage();
})

function previewImage() {
    $("#chooseImg").change(function (e) {
        var url = $("#chooseImg").val();
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (chooseImg.files && chooseImg.files[0] && (ext == 'gif' || ext == 'jpg' || ext == 'jfif' || ext == 'png' || ext == 'bmp')) {
            var reader = new FileReader();
            reader.onload = function () {
                var output = document.getElementById('previewImg');
                output.src = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
        }
    });
}

function AddEmployee() {

    var objData = {
        FirstName: $('#FirstName').val(),
        LastName: $('#LastName').val(),
        Email: $('#Email').val(),
        DateofBirth: $('#DateofBirth').val(),
        Gender: $('#Gender').val(),
        DepartmentId: $('#DepartmentId').val(),
        PhotoPath: $('#chooseImg').val()
    }
    $.ajax({
        url: '/Ajax/AddEmployee',
        type: 'Post',
        data: objData,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8;',
        dataType: 'json',

        success: function (result) {

            alert(result);
            ResetModal();
            ShowEmployeeData();
            HideModal();

        },
        error: function () {
            alert("Failed to add employee");
        }

    });

}

function HideModal() {
    $('#employeeModal').modal('hide');
}
function ResetModal() {
    $('#EmployeeId').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#Email').val('');
    $('#DateofBirth').val('');
    $('#Gender').val('');
    $('#DepartmentId').val('');
    $('#chooseImg').val('');
    $('#previewImg').attr('src', 'https://placehold.jp/150*150.png');
}

function Delete(id) {
    if (confirm(
        'Are you sure you want to delete this record?'
    )) {
        $.ajax({
            url: '/Ajax/Delete?id=' + id,
            success: function (result) {
                alert(result);
                ShowEmployeeData();
            },
            error: function () {
                alert("Can't delete record");
            }
        })
    }
}

function Edit(id) {
    $.ajax({
        url: '/Ajax/Edit?id=' + id,
        type: 'Get',
        contentType: 'application/json; charset=utf-8;',
        dataType: 'json',
        success: function (result) {
            
            $('#employeeModal').modal('show');
            $('#empId').show();
            ShowDepartmentData(result.departmentId);                
            $('#EmployeeId').val(result.employeeId);
            $('#FirstName').val(result.firstName);
            $('#LastName').val(result.lastName);
            $('#Email').val(result.email);
            $('#DateofBirth').val(result.dateofBirth.split('T')[0]);
            $('#Gender').val(result.gender);
            //$('#DepartmentId').val(result.departmentId);
            $('#previewImg').attr("src", "/images/" + result.photoPath);
            $('#btnAdd').hide();
            $('#btnUpdate').show();
            $('#modalHeader').html('Update Employee');

            
        },
        error: function () {
            alert("Error while updating");
        }
    })
}

function UpdateEmployee() {
    
    var objData = {
        EmployeeId: $('#EmployeeId').val(),
        FirstName: $('#FirstName').val(),
        LastName: $('#LastName').val(),
        Email: $('#Email').val(),
        DateofBirth: $('#DateofBirth').val(),
        Gender: $('#Gender').val(),
        DepartmentId: $('#DepartmentId').val(),
        PhotoPath: $('#chooseImg').val()
    }
    $.ajax({
        url: '/Ajax/Edit',
        type: 'Post',
        data: objData,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8;',
        dataType: 'json',

        success: function (result) {

            alert(result);
            ResetModal();
            ShowEmployeeData();
            HideModal();

        },
        error: function () {
            alert("Failed to add employee");
        }

    });

}
