var fileBase64 = null;

function ShowLoading(target, effect = 'win8', message = '', callback = null) {
    $(target).waitMe({
        effect: effect,
        text: message !== undefined || message !== null ? message : '',
        bg: 'rgba(255,255,255,0.7)',
        color: '#00558b',
        maxSize: '',
        waitTime: -1,
        textPos: 'vertical',
        fontSize: '',
        source: '',
        onClose: callback !== undefined && callback !== null ? callback : function () {
        }
    });
}

function ShowNotif(message, type) {
    $.notify(message, type);
}

function AlertMessage(title, message, callback, icon) {
    swal({
        title: title,
        text: message,
        showCancelButton: false,
        icon: icon,
        buttons: {
            confirm: {
                text: "OK",
                value: true,
                visible: true,
                className: "btn-primary",
                closeModal: true,
            }
        }
    }).then(isConfirm => {
        if (callback != undefined && callback != null && callback != "")
            return callback(isConfirm);
    });
}

function ConfirmMessage(message, callback) {
    swal({
        text: message,
        showCancelButton: true,
        buttons: {
            cancel: {
                text: "No",
                value: null,
                visible: true,
                className: "btn-danger",
                closeModal: true,
            },
            confirm: {
                text: "Yes",
                value: true,
                visible: true,
                className: "btn-primary",
                closeModal: true,
            }
        }
    }).then(isConfirm => {
        return callback(isConfirm);
    });
}

function SetPagination(pagination_id, page, totalPage, function_name, function_param) {
    if (page == 1) {
        $(`#${pagination_id}`).append('<li class="paginate_button page-item"><a href="#" class="page-link" style="font-size: 20px; padding-top: 5px;"><span class="text-primary" aria-hidden="true">&laquo;</span></a></li>');
    } else {
        var firstPageParam = function_param !== undefined ? `('${function_param}', 1)` : `(1)`;
        var pageParam = function_param !== undefined ? `('${function_param}', ${(page - 1)})` : `(${(page - 1)})`;
        $(`#${pagination_id}`).append(`<li class="paginate_button page-item"><a href="#" class="page-link" onclick="${function_name}${pageParam}" style="font-size: 20px; padding-top: 1px;"><span class="text-primary" aria-hidden="true">&laquo;</span></a></li>`);
    }

    var i = page == 1 ? page : page == 2 ? page - 1 : page == 3 ? page - 2 : page == 4 ? page - 3 : page == totalPage ? totalPage - 4 : page == totalPage - 1 ? totalPage - 4 : page == totalPage - 2 ? totalPage - 4 : page == totalPage - 3 ? totalPage - 5 : page == totalPage - 4 ? totalPage - 6 : page - 2;
    var toPage = totalPage <= 5 ? totalPage : page == 1 ? page + 4 : page == 2 ? page + 3 : page == 3 ? page + 2 : page == totalPage ? totalPage : page == totalPage - 1 ? totalPage : page == totalPage - 2 ? totalPage : page == totalPage - 3 ? totalPage - 1 : page + 2;
    for (i; i <= toPage; i++) {
        if (i == page) {
            $(`#${pagination_id}`).append(`<li class="paginate_button page-item"><a href="#" class="page-link"><b>${i}</b></a></li>`);
        } else {
            var pageParam = function_param !== undefined ? `('${function_param}', ${i})` : `(${i})`;

            $(`#${pagination_id}`).append(`<li class="paginate_button page-item"><a href="#" class="page-link" onclick="${function_name}${pageParam}"><b>${i}</b></a></li>`);
        }
    }

    if (page == totalPage) {
        $(`#${pagination_id}`).append('<li class="paginate_button page-item"><a href="#" class="page-link" style="font-size: 20px; padding-top: 5px;"><span class="text-primary" aria-hidden="true">&raquo;</span></a></li>');
    } else {
        var pageParam = function_param !== undefined ? `('${function_param}', ${(page + 1)})` : `(${(page + 1)})`;
        var lastPageParam = function_param !== undefined ? `('${function_param}', ${totalPage})` : `(${totalPage})`;

        $(`#${pagination_id}`).append(`<li class="paginate_button page-item"><a href="#" class="page-link" onclick="${function_name}${pageParam}" style="font-size: 20px; padding-top: 1px;"><span class="text-primary" aria-hidden="true">&raquo;</span></a></li>`);
    }
}

function SetTableData(haveData, colspan, el, data, callback) {
    var el_tbody = el != undefined || el != null ? el.tbody : '#tbody';
    var el_from = el != undefined || el != null ? el.from : '#from_page';
    var el_to = el != undefined || el != null ? el.to : '#to_page';
    var el_total = el != undefined || el != null ? el.total : '#total_count';
    var el_pagination = el != undefined || el != null ? el.pagination : '#pagination';
    var el_item_pagination = el != undefined || el != null ? el.item_pagination : 'item-page';

    var from = haveData ? ((data.page - 1) * data.pageSize) + 1 : 0;
    var totalPage = haveData ? data.count % data.pageSize == 0 ? Math.floor(data.count / data.pageSize) : Math.floor(data.count / data.pageSize) + 1 : 0;
    var to = haveData ? totalPage == data.page ? data.count : (data.page * data.pageSize) : 0;

    $(el_from).html(from);
    $(el_to).html(to);
    $(el_total).html(haveData ? data.count : 0);
    $(el_pagination).html(`<ul class="pagination pagination-sm" id="pagination-page_${el_item_pagination}"></ul>`);

    if (haveData) {
        SetPagination(`pagination-page_${el_item_pagination}`, data.page, totalPage, data.function_name, data.function_param);
        return callback(from);
    } else {
        $(el_tbody).html('');
        $(el_tbody).append(`<tr><td colspan="${colspan}"><center>Data Not Found!</center></td></tr>`);
        $(`#pagination-page_${el_item_pagination}`).append(`
            <li class="paginate_button page-item disabled" id=""><a class="page-link" style="font-size: 20px; padding-top: 1px;">&laquo;</a></li>
    		<li class="paginate_button page-item active"><a class="page-link">1</a></li>
    		<li class="paginate_button page-item disabled" id=""><a class="page-link" style="font-size: 20px; padding-top: 1px;">&raquo;</a></li>
        `);
    }
}

function NumberToRupiah(value) {
    var result = "0";
    if (value != undefined && value != null && value != "" && value != 0) {
        var rupiah = '';
        var angkarev = value.toString().split('').reverse().join('');
        for (var i = 0; i < angkarev.length; i++)
            if (i % 3 == 0) rupiah += angkarev.substr(i, 3) + '.';
        result = rupiah.split('', rupiah.length - 1).reverse().join('');

    }
    return result;
}

function AlertNotification(title = null, message = null, buttonText = null, type = null) {

    if (title == null) {
        title = "Info";
    }
    if (message == null) {
        message = "No Message";
    }
    if (buttonText == null) {
        buttonText = "Okay";
    }
    if (type == null) {
        type = "info";
    }

    cuteAlert({
        type: type,
        title: title,
        message: message,
        buttonText: buttonText
    })
}

function AlertConfirmation(title = null, message = null, callback = null) {
    cuteAlert({
        type: "question",
        title: title,
        message: message,
        confirmText: "Okay",
        cancelText: "Cancel"
    }).then((e) => {
        if (e == ("confirm")) {
            if (callback != null)
                return callback("Done");
        } 
    })
}


function TurnOnCamera() { //WAJIB MENGGUNAKAN ELEMENT VIDEO (<video></video>)
    var elemntIdCamera = "capturevideo";
    let videoCapture = document.getElementById(elemntIdCamera);
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        navigator.mediaDevices.getUserMedia({
            video: {
                facingMode: 'environment'
            }
        }).then(function (stream) {
            window.localStream = stream;
            videoCapture.srcObject = stream;
            videoCapture.play();
            activateCamera();
        }).catch(e => {
            console.log("Please Allow: Use Your Camera!");
            $('#mdlCamera').modal('show');
        });
        
    }
}

function TurnOffCamera() {
    $('#mdlCamera').modal('hide');
    localStream.getTracks().forEach(function (track) {
        if (track.readyState == 'live' && track.kind === 'video') {
            track.stop();
        }
    });
}

function clearPicture(el) {
    $(`#${el}`).html(`
        <div class="d-flex justify-content-center" style="height: 250px; align-items: center; background-color: rgba(0,0,0,0.1); border-radius: 5px;">
            <span class="mdi mdi-camera-burst mdi-36px"></span>
        </div>
    `);
    fileBase64 = null;
}

function CaptureCamera(elementOutput) { //HARUS ID JANGAN CLASS
    var elementInput = "capturevideo";
    $(`#${elementOutput}`).html("");
    $(`#${elementOutput}`).html(`
        <canvas id="canvas-${elementOutput}" style="height: 250px; width: 100%;"></canvas>
    `);
    let videoCapture = document.getElementById(elementInput);
    var height = videoCapture.videoHeight;
    var width = videoCapture.videoWidth;

    // Set the size of the canvas to match the video
    let canvas = document.getElementById("canvas-"+elementOutput);
    canvas.width = width;
    canvas.height = height;
    //$('#' + elementOutput).hide();

    // Draw the image
    let ctx = canvas.getContext('2d');
    ctx.drawImage(videoCapture, 0, 0, width, height);
    TurnOffCamera();
    fileBase64 = canvas.toDataURL();
}