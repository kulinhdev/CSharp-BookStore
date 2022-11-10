var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#dtProduct').DataTable({
        "ajax": {
            "url": "/Admin/Products/GetAll"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            {
                "data": "price", "render": function (price, type, full) {
                    return `<b><del class="text-danger">$${full['price']}</del> - $${full['salePrice']}</b>`
                }
            },
            {
                "data": "imagePath", "render": function (imagePath) {
                    return `<img width="80px" src="${imagePath}">`
                }
            },
            { "data": "category.name" },
            { "data": "author.name" },
            { "data": "releaseDate" },
            {
                "data": "id", "render": function (id) {
                    return `<a class="btn btn-warning" href="/Admin/Products/Upsert/${id}"><i class="bi bi-pencil"></i></a>
                            <a class="btn btn-danger" onclick="deleteProduct('/Admin/Products/Delete/${id}')"><i class="bi bi-trash"></i></a>`
                }
            },
        ]
    });
}

function deleteProduct(url) {
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
                type: "DELETE",
                success: function (data) {
                    console.log(data);
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}