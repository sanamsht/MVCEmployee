$(document).ready(function () {
   
    GetEmployee();

    
})
function GetEmployee() {
    $.ajax({
        url: '/Home/GetEmployeeData',
        type: 'Get',
        dataType: 'json',
        success: OnSuccess

        
    })
}
function OnSuccess(response) {
    $('#myTable').DataTable({
        bProcessing: true,
        bLengthChange: true,
        lengthMenu: [[5, 10, 25, -1], [5, 10, 25, "All"]],
        bfilter: true,
        bSort: true,
        bPaginate: true,
        data: response,
        columns: [
            
            {
                data: 'FirstName',
                render: function (data, type, row, meta) {
                    return row.firstName
                }
            },
            {
                data: 'LastName',
                render: function (data, type, row, meta) {
                    return row.lastName
                }
            },
            {
                data: 'Email',
                render: function (data, type, row, meta) {
                    return row.email
                }
            },
            {
                data: 'DateofBirth',
                render: function (data, type, row, meta) {
                    return row.dateofBirth
                }
            },
            {
                data: 'Gender',
                render: function (data, type, row, meta) {
                    return row.gender
                }
            },
           
            {
                data: 'PhotoPath',
                render: function (data, type, row, meta) {
                    return row.photoPath
                }
            }
           

        ]
    });
}
