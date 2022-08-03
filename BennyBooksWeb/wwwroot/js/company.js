var dataTable;
// Used by the Index.cshtml from the Company table View in BennyBooksWeb and that is getting it data from the ProductController
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        // Column number needs to match what is in the index.cshtml. Data for our table
        "columns": [ // Case sensitive, so check the API output .../admin/company/getall
            { "data": "Name", "width": "13%" },
            { "data": "streetaddress", "width": "13%" },
            { "data": "city", "width": "13%" },
            { "data": "state", "width": "13%" },
            { "data": "postalcode", "width": "13%" },
            { "data": "phonenumber", "width": "13%" },
            {
                "data": "id",
                "render": function (data) {// Buttons using id
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Company/Upsert?id=${data}"
                            class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                            <a onClick=Delete('/Admin/Company/Delete/${data}')
                            class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                        
                        </div>
                    `
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) { // used by Delete APIcall in productcontroller, base off the return
                        dataTable.ajax.reload(); // Reload our table once its deleted
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}