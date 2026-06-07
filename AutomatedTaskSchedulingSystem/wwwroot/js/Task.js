
var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: '/Admin/Task/GetAll'
        },
       
        columns: [
            { data: 'taskName', width: '20%' },
            { data: 'location.name', width: '20%' },
            { data: 'minEmployees', width: '10%' },
            { data: 'maxEmployees', width: '10%' },
            {
                data: 'taskID',
                render: function (data, type, row) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Task/Upsert/${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>

                            <a onclick="Delete('/Admin/Task/Delete/${data}', '${row.taskName}')" 
                               class="btn btn-danger" style="cursor:pointer">
                                <i class="bi bi-trash-fill"></i> Delete
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


function Delete(url, taskName) {
    Swal.fire({
        title: `Are you sure you want to delete ${taskName}?`, 
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



