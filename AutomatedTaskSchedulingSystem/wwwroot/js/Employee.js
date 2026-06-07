


var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: '/Employ/Employees/GetAll'
        },

        columns: [
            { data: 'empID', width: '10%' },
            {
                data: null,
                render: function (data, type, row) {
                    return row.firstName + ' ' + row.lastName;
                },
                width: '25%'
            },
            { data: 'sex', width: '10%' },
            { data: 'position.name', width: '20%' },
            {
                data: 'id',
                render: function (data, type, row) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Employ/Employees/Upsert/${data}" class="btn btn-primary mx-2">
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


function Delete(url, employeeName) {
    Swal.fire({
        title: `Are you sure you want to delete ${employeeName}?`,
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}



