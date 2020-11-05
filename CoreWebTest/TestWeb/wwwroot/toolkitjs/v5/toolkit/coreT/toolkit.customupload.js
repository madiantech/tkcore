// --------------------------------
// Toolkit.list
// --------------------------------
(function ($) {
    if (!$ || !window.Toolkit) return;
    // ----------------------------
    Toolkit.namespace('Toolkit.upload');

    Toolkit.upload.successUploaded = function (data, elm) {
        if (data && data.Result) {
            var result = data.Result.Result;
            if (result === "Success") {
                var upload = data.UploadInfo;
                var ctrl = elm.find('[data-control]');
                ctrl.data("upload", upload);
                elm.removeClass("uploading");// 移除上传中的标记
                // Toolkit.ui._showUploadItem(upload, elm, file, elm.find(".upload-status"));
                Toolkit.upload.showUploadItem(upload, elm);
            }
            else if (result === "Error") {
                // Toolkit.page.showError(data.Result.Message);
                elm.remove();
            }
        }
    };
    Toolkit.upload.showUploadItem = function (upload, elm) {
        elm.find('[data-control]').html(`
            <a href="${upload.WebPath}" target="_blank" class="text-center" title="${upload.FileName}">
                <i class="file-icon glyphicon glyphicon-file"></i>
                <p>${upload.FileName}</p>
            </a>
            <button type="button" class="file-remove btn btn-danger btn-xs glyphicon glyphicon-remove"></button>
            `);
        elm.find("button.file-remove").click(Toolkit.upload.deleteUploaded);
        elm.trigger("afterItemSelected", { "Upload": upload, "Element": elm });
    };

    Toolkit.upload.deleteUploaded = function () {
        $(this).parents("div.upload-status-new").remove();
    };

    Toolkit.upload.upload = function (event, file, i) {
        // 添加上传中的对象
        var uploadingDom = $(event).siblings('[temp]').clone().show().removeAttr("temp");
        $(event).siblings('button[name=uploadBtn]').before(uploadingDom);

        if (window.FileReader) {
            var reader = new FileReader();
            reader.onloadend = function (e) {
                //if (ctrl.attr("data-view") == "true")
                //    Toolkit.ui._showUploadedItem(e.target.result, resDiv.find("div.responseImage"));
            };
            reader.readAsDataURL(file);
        }
        var formdata = false;
        if (window.FormData) {
            formdata = new FormData();
        }
        if (formdata) {
            formdata.append("Filedata", file);
            // var url = Toolkit.page.getAbsoluteUrl("c/plugin/C/Upload");
            var url = $(event).data("url");
            if (url === undefined || url === "") {
                url = Toolkit.page.getAbsoluteUrl("c/plugin/C/Upload");
            }
            else
                url = Toolkit.page.getAbsoluteUrl(url);
            return $.ajax({
                url: url,
                type: "Post",
                data: formdata,
                processData: false,
                contentType: false,
                success: function (res) {
                    Toolkit.upload.successUploaded(res, uploadingDom);
                },
                error: function (res) {
                    Toolkit.page.showError('上传失败，请稍后重试。');
                    uploadingDom.remove();
                }
            });
        }
    };
    $(document).ready(function () {
        $('button[name=uploadBtn]').click(e => {
            $(e.currentTarget).siblings('input[name=fileCtrl]').trigger('click');
        });

        $('button[name=uploadBtn]').parent().find('[data-control]').each((i, e) => {
            var elm = $(e)
            var value = elm.attr("data-value");
            var response = elm.parents(".upload-status-new");
            if (value === undefined) {
                response.remove();
            } else if (value !== "") {
                var upload = JSON.parse(value)
                elm.data("upload", upload);
                Toolkit.upload.showUploadItem(upload, response);
            };
            if (!window.FormData) {
                alert("系统不支持HTML5上传");
                elm.siblings('input[name=fileCtrl]').attr("disabled", "disabled");
            }
        })
        // _onFileChange
        $('input[name=fileCtrl]').change(e => {
            var fileinput = $(e.currentTarget);
            var len = e.currentTarget.files.length;
            var maxsize = parseFloat(fileinput.data('maxsize'));

            for (var i = 0; i < len; i++) {
                var file = e.currentTarget.files[i]
                if (maxsize && file.size > maxsize) {
                    alert("请上传小于等于" + (maxsize / 1024 / 1024).toFixed(1) + "M的图片");
                    return false;
                }
                Toolkit.upload.upload(e.currentTarget, file, i)
            }
            fileinput.replaceWith(fileinput.val('').clone(true));
        });
    });
})(jQuery);