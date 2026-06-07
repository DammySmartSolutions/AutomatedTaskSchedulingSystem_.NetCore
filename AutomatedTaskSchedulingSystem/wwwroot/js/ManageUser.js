var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: '/Admin/ManageUser/GetAll',
            error: function (xhr) {
                console.log(xhr.responseText);
                alert(xhr.status + " - " + xhr.statusText);
            }
        },
        columns: [
            { data: 'empID', width: '15%' },
            { data: 'email', width: '20%' },
            { data: 'role', width: '15%' },
            {
                data: 'emailConfirmed',
                render: function (data) {
                    return data
                        ? '<span class="badge bg-success">Confirmed</span>'
                        : '<span class="badge bg-warning text-dark">Not Confirmed</span>';
                },
                width: '15%'
            },
            {
                data: 'isLocked',
                render: function (data) {
                    return data
                        ? '<span class="badge bg-danger">Locked</span>'
                        : '<span class="badge bg-success">Active</span>';
                },
                width: '10%'
            },
            {
                data: 'id',
                render: function (data, type, row) {
                    let lockButton = row.isLocked
                        ? `<a onclick="UnlockUser('${data}')" class="btn btn-sm btn-success me-1">
                                <i class="bi bi-unlock"></i> Unlock
                           </a>`
                       
                        : `<a onclick="LockUser('${data}')" class="btn btn-sm btn-warning me-1">
                                <i class="bi bi-lock"></i> Lock
                           </a>`;

                    return `
                        <div class="btn-group" role="group">
                            <a href="/Admin/ManageUser/EditRole/${data}" class="btn btn-sm btn-primary me-1">
                                <i class="bi bi-person-gear"></i> Edit Role
                            </a>

                            <a href="/Admin/ManageUser/ResetPassword/${data}" class="btn btn-sm btn-secondary me-1">
                                <i class="bi bi-key"></i> Reset Password
                            </a>

                            ${lockButton}

                            <a onclick="DeleteUser('/Admin/ManageUser/Delete/${data}', '${row.empID}')" 
                               class="btn btn-sm btn-danger">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>
                    `;
                },
                width: '25%'
            }
        ],
        dom: 'frtipB',
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print']
    });

    


}
function LockUser(id) {
    $.post(`/Admin/ManageUser/LockUser/${id}`, function (data) {
        if (data.success) {
            toastr.success(data.message);
            dataTable.ajax.reload();
        } else {
            toastr.error(data.message);
        }
    }).fail(function () {
        toastr.error("Error while locking user.");
    });
}

function UnlockUser(id) {
    $.post(`/Admin/ManageUser/UnlockUser/${id}`, function (data) {
        if (data.success) {
            toastr.success(data.message);
            dataTable.ajax.reload();
        } else {
            toastr.error(data.message);
        }
    }).fail(function () {
        toastr.error("Error while unlocking user.");
    });
}

function DeleteUser(url, empID) {
    Swal.fire({
        title: `Delete user ${empID}?`,
        text: "This action cannot be undone.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete",
        confirmButtonColor: "#d33"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
