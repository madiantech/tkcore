var successUploaded = function (data, elm) {
    if (data && data.Result) {
        var result = data.Result.Result;
        if (result === "Success") {
            var upload = data.UploadInfo;
            var ctrl = elm.find('[data-control=Upload1]');
            ctrl.data("upload", upload);
            elm.removeClass("uploading");// 移除上传中的标记
            // Toolkit.ui._showUploadItem(upload, elm, file, elm.find(".upload-status"));
            showUploadItem(upload, elm);
        }
        else if (result === "Error") {
            // Toolkit.page.showError(data.Result.Message);
            elm.remove();
        }
    }
};
// _showUploadItem
var showUploadItem = function (upload, elm) {
    // info.html('<span><a href="' + upload.WebPath + '" target="_blank">' + upload.FileName + '</a><button type="button" class="close easysearch-close" aria-hidden="true">&times;</button></span>');
    // info.find("button.close").click(Toolkit.ui._deleteUploaded);
    // elm.empty();// 先清空上传中留下的内容
    // elm.find('input[name=Id]').val('')
    // elm.find('input[name=WoId]').val('')
    elm.find('[data-control=Upload1]').html(`
            <a href="${upload.WebPath}" target="_blank" class="text-center">
                <i class="file-icon glyphicon glyphicon-file"></i>
                <p>${upload.FileName}</p>
            </a>
            <button type="button" class="file-remove btn btn-danger btn-xs glyphicon glyphicon-remove"></button>
            `);
    elm.find("button.file-remove").click(deleteUploaded);
    elm.trigger("afterItemSelected", { "Upload": upload, "Element": elm });
};
var deleteUploaded = function () {
    $(this).parents("div.upload-status").remove();
};

$(document).ready(function () {
    $('#uploadBtn').click(e => {
        $('#fileCtrl').trigger('click');
    });

    $('#uploadBtn').parent().find('[data-control=Upload1]').each((i, e) => {
        var elm = $(e)
        var value = elm.attr("data-value");
        var response = elm.parents(".upload-status")
        if (value === undefined) {
            response.remove();
        } else {
            var upload = JSON.parse(value)
            elm.data("upload", upload);
            showUploadItem(upload, response);
        }
        if (!window.FormData) {
            alert("系统不支持HTML5上传");
            $('#fileCtrl').attr("disabled", "disabled");
        }
    })
    // _onFileChange
    $('#fileCtrl').change(e => {
        // var fileinput = $(this)
        var fileinput = $('#fileCtrl');
        // 添加上传中的对象
        var uploadingDom = $(`<div class="upload-status uploading table-row">
            <input type="hidden" name="Id" />
            <input type="hidden" name="WoId" />
            <div data-control="Upload1" data-fileSize="Size" data-serverPath="FilePath" data-contentType="ContentType" name="FileName">
                <a class="text-center">
                    <i class="file-icon icon-spinner icon-spin mr5 ml5"></i>
                    <p>正在上传……</p>
                </a>
            </div>
        </div>`);
        $('#uploadBtn').before(uploadingDom);

        var formdata = false;
        if (window.FormData) {
            formdata = new FormData();
        }

        var len = e.currentTarget.files.length;
        for (var i = 0; i < len; i++) {
            var file = e.currentTarget.files[i];

            if (window.FileReader) {
                var reader = new FileReader();
                reader.onloadend = function (e) {
                    //if (ctrl.attr("data-view") == "true")
                    //    Toolkit.ui._showUploadedItem(e.target.result, resDiv.find("div.responseImage"));
                };
                reader.readAsDataURL(file);
            }
            if (formdata) {
                formdata.append("Filedata", file);
            }
        }
        if (formdata) {
            var url = Toolkit.page.getAbsoluteUrl("c/plugin/C/Upload");
            $.ajax({
                url: url,
                type: "Post",
                data: formdata,
                processData: false,
                contentType: false,
                success: function (res) {
                    successUploaded(res, uploadingDom);
                },
                error: function (res) {
                    Toolkit.page.showError('上传失败，请稍后重试。');
                }
            });
            fileinput.replaceWith(fileinput.val('').clone(true));
        }
        // change ajax 上传
        //setTimeout(function () {
        //    // 上传成功
        //    successUploaded({ Result: { Result: 'Success' }, UploadInfo: { info: 'info' } }, uploadingDom)
        //    // 上传失败
        //    // uploadingDom.remove()

        //    // 无论成功与否 都清空文件输入框
        //    fileinput.replaceWith(fileinput.val('').clone(true));
        //}, 3000);
    });
});