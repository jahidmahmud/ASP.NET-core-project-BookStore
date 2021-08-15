var dataTable;
$(document).ready(function () {
    loadTable();
});
function loadTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "10%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "postalCode", "width": "10%" },
            { "data": "phone", "width": "10%" },
            {
                "data": "isAuthorizaCompany",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" disabled checked>`
                    }
                    else{
                        return `<input type="checkbox" disabled >`
                    }
                },
                "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Company/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-plus"></i>
                                </a>
                                <a onclick=Delete("/Admin/Company/Delete/${data}") class="btn btn-danger text-white">
                                    <i class="fas fa-trash-alt" style="cursor:pointer"></i>
                                </a>
                            </div>`;
                }, "width": "30%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure want to delete",
        text: "You will lose this record permanently",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "Delete",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}