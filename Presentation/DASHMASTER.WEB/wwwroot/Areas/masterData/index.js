$(document).ready(function () {
	InilitializeProduct();
});

function InilitializeProduct() {
    getListProduct();
}

function getListProduct(page) {
    page = page != undefined ? page : 1;
    var pageSize = 10;
    var element = StandardElement('product');
    var params = {
        "Filter": [
            {
                "Field": "string",
                "Search": "string"
            }
        ],
        "Sort": {
            "Field": "string",
            "Type": 0
        },
        "Start": page,
        "Length": pageSize
    }

    RequestData('POST', '/v1/Product/list', null, null, JSON.stringify(params), function (data) {
		if (data.Succeeded) {
            $(element.tbody).html('');
            if (data.List.length > 0) {
                SetTableData(true, 0, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListProduct'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
								<td>
									<div class="form-check form-check-sm form-check-custom form-check-solid">
										<input class="form-check-input" type="checkbox" value="1" />
									</div>
								</td>
								<td>
									<div class="d-flex align-items-center">
										<a href="apps/ecommerce/catalog/edit-product.html" class="symbol symbol-50px">
											<span class="symbol-label" style="background-image:url(assets/media//stock/ecommerce/1.png);"></span>
										</a>
										<div class="ms-5">
											<a href="apps/ecommerce/catalog/edit-product.html" class="text-gray-800 text-hover-primary fs-5 fw-bold" data-kt-ecommerce-product-filter="product_name">Product 1</a>
										</div>
									</div>
								</td>
								<td class="text-end pe-0">
									<span class="fw-bold">${item.Name}</span>
								</td>
								<td class="text-end pe-0" data-order="36">
									<span class="fw-bold ms-3">36</span>
								</td>
								<td class="text-end pe-0">53</td>
								<td class="text-end pe-0" data-order="rating-4">
									<div class="rating justify-content-end">
										<div class="rating-label checked">
											<i class="ki-duotone ki-star fs-6"></i>
										</div>
										<div class="rating-label checked">
											<i class="ki-duotone ki-star fs-6"></i>
										</div>
										<div class="rating-label checked">
											<i class="ki-duotone ki-star fs-6"></i>
										</div>
										<div class="rating-label checked">
											<i class="ki-duotone ki-star fs-6"></i>
										</div>
										<div class="rating-label">
											<i class="ki-duotone ki-star fs-6"></i>
										</div>
									</div>
								</td>
								<td class="text-end pe-0" data-order="Inactive">
									<div class="badge badge-light-danger">Inactive</div>
								</td>
								<td class="text-end">
									<a href="#" class="btn btn-sm btn-light btn-flex btn-center btn-active-light-primary" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end">
										Actions
										<i class="ki-duotone ki-down fs-5 ms-1"></i>
									</a>
									<div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-semibold fs-7 w-125px py-4" data-kt-menu="true">
										<div class="menu-item px-3">
											<a href="apps/ecommerce/catalog/edit-product.html" class="menu-link px-3">Edit</a>
										</div>
										<div class="menu-item px-3">
											<a href="#" class="menu-link px-3" data-kt-ecommerce-product-filter="delete_row">Delete</a>
										</div>
									</div>
								</td>
							</tr>
                        `); count++;
                    });
                });
            } else {

            }
        } else if (data.Code == 404) {

        } else {

        }
    });
}

function AddProduct() {

}