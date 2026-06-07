



var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: '/Employ/EmployeeAvailability/GetAll'
        },

        columns: [
            {
                data: null,
                render: function (data, type, row) {

                    if (!row.employee)
                        return '';

                    return `${row.employee.firstName}
                ${row.employee.lastName}
                - ${row.employee.empID}`;
                },
                width: '25%'
            },

            {
                'data': 'availDate',
                'render': function (data, type, row) {
                    if (!data) {
                        return '';
                    }

              
                    return moment(data).format('DD-MM-YYYY');


                },
            },
            { data: 'avail', width: '10%' },
            {
                data: 'availID',
                render: function (data, type, row) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Employ/EmployeeAvailability/Upsert/${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>

                            
                        </div>
                `;
                },
                width: '20%'
            }
        ],
        dom: 'frtipB',
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
    });
}


// function Delete(url, employeeName) {
//     Swal.fire({
//         title: `Are you sure you want to delete ${employeeName}?`,
//         text: "You won't be able to revert this!",
//         icon: "warning",
//         showCancelButton: true,
//         confirmButtonColor: "#3085d6",
//         cancelButtonColor: "#d33",
//         confirmButtonText: "Yes, delete it!"
//     }).then((result) => {
//         if (result.isConfirmed) {
//             $.ajax({
//                 url: url,
//                 type: 'DELETE',
//                 success: function (data) {
//                     if (data.success) {
//                         dataTable.ajax.reload();
//                         toastr.success(data.message);
//                     } else {
//                         toastr.error(data.message);
//                     }
//                 }
//             });
//         }
//     });
// }



