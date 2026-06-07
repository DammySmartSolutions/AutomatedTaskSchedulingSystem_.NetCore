


var dataTable;

$(document).ready(function () {
    loadDataTable();
});



function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/location/getall' },
        "columns": [
            { "data": "name", "width": "50%" },
            
            {
                data: 'locId',
                render: function (data, type, row) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/location/Upsert/${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>

                            <a onclick="Delete('/Admin/location/Delete/${data}', '${row.name}')" 
                               class="btn btn-danger" style="cursor:pointer">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                `;
                },

                "width": "20%"
            }
        ],
        dom: 'frtipB',
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
    });
}

function Delete(url, name) {
    Swal.fire({
        title: `Are you sure you want to delete ${name}?`,
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