$(document).ready(function () {

    GetAllData();
})
var table;
function GetAllData() {
   table = $('#tblData').DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        lengthMenu: [[5, 10, 15, 30, 50, 100, -1], [5, 10, 15, 30, 50, 100, 'All']],
        pagelength: 5,
        language: {
            lengthMenu: "Display _MENU_ records/page",
            info: "Showing page _PAGE_ of _PAGES_ records",
            infoEmpty: "No records available",
            zeroRecords: "Sorry! Data not availalble"
        },
        ajax: {

            type: "Post",
            url: '/ServerDT/GetData',
            data: 'json'
        },
        columns: [

            { "data": "employeeId", "name": "EmployeeId", "autowidth": true },
            { "data": "firstName", "name": "FirstName", "autowidth": true },
            { "data": "lastName", "name": "LastName", "autowidth": true },
            { "data": "email", "name": "Email", "autowidth": true },
            { "data": "dateofBirth", "name": "DateofBirth", "autowidth": true },
            { "data": "gender", "name": "Gender", "autowidth": true },
            { "data": "departmentId", "name": "DepartmentId", "autowidth": true },
            { "data": "photoPath", "name": "PhotoPath", "autowidth": true },
            {
                render: function (data, type, row, meta) {
                    return '<a href="#" class="btn btn-sm btn-danger m-1 p-1">Delete</a>'
                }
            }
        ],
        columnDefs: [

            {
                targets: [0],
                visible: false,
                searchable: false,
            }]
    });

}

$(window).resize(function () {
    let height = $(this).height();
    if (height < 200) {
        table.page.len(5).draw();
    }
    else if (height < 400) {
        table.page.len(10).draw();
    }
    else if (height < 600) {
        table.page.len(15).draw();
    }
    else if (height < 800) {
        table.page.len(30).draw();
    }
    else if (height < 1000) {
        table.page.len(50).draw();
    }
    else  {
        table.page.len(100).draw();
    }

})