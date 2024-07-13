function ListReferensi(referensi_name, url, params, select_container, select_data, setdata, callback, with_code, show_title) {
    RequestData("GET", url, select_container, select_data, params, function (data) {
        if (data.succeeded) {
            if (data.list.length > 0) {
                var counter = 0;
                var selected = "";
                if (show_title)
                    $(select_data).append(`<option value="" data-kode="" selected disabled>----PILIH ${referensi_name}----</option>`);

                data.list.forEach(function (item) {
                    var text = item.name == undefined ? item.nama : item.name;
                    if (with_code != undefined || with_code != null || with_code == true)
                        text = (item.kode == undefined ? item.id : item.kode) + " - " + (item.name == undefined ? item.nama : item.name);
                    if (setdata != null || setdata != "") {
                        if (setdata == "any")
                            selected = counter == 0 ? "selected" : "";
                        else
                            selected = item.id == setdata ? "selected" : "";
                    }
                    else {
                        if (show_title == null || show_title == false)
                            selected = counter == 0 ? "selected" : "";
                    }
                    $(select_data).append(`<option value="${item.id}" data-detail='${JSON.stringify(item)}' ${selected}>${text}</option>`);
                    counter++;
                });
            } else {
                $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
                ShowNotif("Data " + referensi_name + " Not Found!", "warning");
                data.succeeded = false;
            }
        } else {
            $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
        if (callback != undefined || callback != null) {
            return callback(data.succeeded);
        }
    }, false, "win8_linear");
}

function ListUnitOfMaterial(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Unit Of Material", "GeneralReference/list_table_referensi?type=1", null, select_container, select_data, setdata, callback, with_code, show_title);
}

function ListSupplier(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Supplier", "MasterSupplier/list_master_supplier", null, select_container, select_data, setdata, callback, with_code, show_title);
}

function ListPakan(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Pakan", "MasterPakan/list_master_pakan", null, select_container, select_data, setdata, callback, with_code, show_title);
}

function ListKandang(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Kandang", "GeneralReference/list_table_referensi?type=2", null, select_container, select_data, setdata, callback, with_code, show_title);
}

function ListTypeKambing(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Jenis Kelamin", "GeneralReference/list_table_referensi?type=3", null, select_container, select_data, setdata, callback, with_code, show_title);
}

function ListJenisKambing(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Jenis Kambing", "GeneralReference/list_table_referensi?type=4", null, select_container, select_data, setdata, callback, with_code, show_title);
}

function ListKambingJantan(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Kambing Jantan", "GeneralReference/list_kambing_by_type?type=1", null, select_container, select_data, setdata, callback, with_code, show_title);
}

function ListKambingBetina(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Kambing Betina", "GeneralReference/list_kambing_by_type?type=2", null, select_container, select_data, setdata, callback, with_code, show_title);
}

function ListKambing(select_container, select_data, setdata, callback, with_code, show_title) {
    ListReferensi("Kambing", "GeneralReference/list_kambing_by_type", null, select_container, select_data, setdata, callback, with_code, show_title);
}
