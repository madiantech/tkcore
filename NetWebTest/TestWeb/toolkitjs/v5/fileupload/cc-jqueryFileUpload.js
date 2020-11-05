'use strict'

// 设置后台文件的路径
var aspxUrl = '../TestUpload.aspx'

// Initialize the jQuery File Upload widget:
$('#fileupload').fileupload({
    // 取消注释就可以发送跨域的 cookies 即 xhrFields: {widthCredentials: true}
    // Uncomment the following to send cross-domain cookies:
    //xhrFields: {withCredentials: true},
    url: window.aspxUrl,
    acceptFileTypes: /(\.|\/)(gif|jpe?g|png|txt|mp3|rar|flv)$/i
});

// Load existing files: // 在加载图片的时候会显示 .fileupload-processing 是一张正在加载的图，就在四个按钮的右边
$('#fileupload').addClass('fileupload-processing');
$.ajax({
    url: $('#fileupload').fileupload('option', 'url'),
    dataType: 'json',
    context: $('#fileupload')[0]
}).always(function() {
    $(this).removeClass('fileupload-processing'); // 无论$.ajax()返回结果如何，都会取消这个加载图片
}).done(function(result) {
    $(this).fileupload('option', 'done')
        .call(this, $.Event('done'), {
            result: result
        });
});

// 上传事件完成的回调
$('#fileupload').bind('fileuploaddone', function(e, data) {
    $.each(data.result.files, function(index, file) {
        console.log(file)
    })
})

// 删除文件按钮
$(document).delegate('.delete', 'click', function() {
    var fileName = $(this).closest('tr').find('a').attr('download')
    if (fileName) {
        $.ajax({ 
            url: window.aspxUrl,
            data: 'file=' + decodeURIComponent(fileName) + '&delete=true',
            success: function(data) {
                console.log(data)
            }
        })
    }
})
