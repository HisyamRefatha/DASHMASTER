function RequestData(type, url, container, field, params, callback, is_append = false, loader_style = "win8") {
    var formData = new FormData();
    formData.append('Url', url);

    formData.append('Method', type);
    if (params != undefined && params != null && params != "")
        formData.append('Body', JSON.stringify(params));


    var config = {
        async: true,
        type: 'POST',
        contentType: false,
        processData: false,
        url: window.location.origin + "/Request/DoRequest",
        data: formData,
        beforeSend: function (xhr) {
            if (container != undefined && container != null && container != "")
                ShowLoading(container, loader_style);
        },
        error: function (err) {
            if (container != undefined && container != null && container != "")
                $(container).waitMe('hide');
            if (err.responseJSON != undefined && err.responseJSON != null) {
                if (callback != undefined && callback != null && callback != "")
                    return callback(err.responseJSON);
                else
                    ShowNotif(err.responseJSON.message, "error");
            } else
                ShowNotif("Something went wrong!", "error");
        },
        success: function (data) {
            if (field != undefined && field != null && field != "" && is_append == false)
                $(field).html('');

            if (container != undefined && container != null && container != "")
                $(container).waitMe('hide');

            if (callback != undefined && callback != null && callback != "")
                return callback(data);
        }
    };
    $.ajax(config);
}