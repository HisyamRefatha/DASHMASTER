function RequestData(type, url, container, field, params, callback, isJson = true) {
    var config = {
        async: true,
        type: type,
        url: _apiUrl + url,
        data: params,
        headers: {
            "Authorization": "Bearer " + _token,
            "Content-Type": 'application/json'
        },
        beforeSend: function (xhr) {
            if (container != undefined && container != null && container != "")
                ShowLoading(container);
        },
        error: function (err) {
            if (container != undefined && container != null && container != "")
                $(container).waitMe('hide');
            if (err.responseJSON != undefined && err.responseJSON != null) {
                if (callback != undefined && callback != null && callback != "")
                    return callback(err.responseJSON);
                else
                    ShowNotif(err.responseJSON.message, "error");
            }
            else
                ShowNotif("Something went wrong!", "error");
        },
        success: function (data) {
            if (field != undefined && field != null && field != "")
                $(field).html('');

            if (container != undefined && container != null && container != "")
                $(container).waitMe('hide');

            if (callback != undefined && callback != null && callback != "")
                return callback(data);
        }
    };

    if (isJson)
        config.dataType = 'json';

    $.ajax(config);
}


function RequestDataFormData(type, url, container, field, params, callback) {

    $.ajax({
        async: true,
        type: type,
        cache: false,
        contentType: false,
        processData: false,
        url: window.location.origin + url,//window.location.origin + url,
        data: params,
        beforeSend: function (xhr) {
            if (container != undefined && container != null && container != "")
                ShowLoading(container);
        },
        error: function (err) {
            if (container != undefined && container != null && container != "")
                $(container).waitMe('hide');

            ShowNotif("Something went wrong", "error");
        },
        success: function (data) {
            if (field != undefined && field != null && field != "")
                $(field).html('');

            if (container != undefined && container != null && container != "")
                $(container).waitMe('hide');

            if (callback != undefined && callback != null && callback != "")
                return callback(data);
        }
    });
}

function DoRequest(type, url, container, field, params, callback, is_append = false, loader_style = "win8") {
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
                //else
                //    ShowNotif(err.responseJSON.message, "error");
            }
            //else
            //    ShowNotif("Something went wrong!", "error");
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