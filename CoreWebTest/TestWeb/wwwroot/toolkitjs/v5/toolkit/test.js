var div = document.getElementById("J_HotItem");
var inputs = div.getElementsByTagName("input");
if (inputs.length > 0) {
    var input = inputs[0];
    input.value = "{0}";
    if (input.value != "") {
        var btn = document.getElementById("J_LinkBuyQiang");
        if (btn != null) {
            btn.click();
            btn.click();
            btn.click();
        }
    }
}
else {
    var btns = div.getElementsByTagName("button");
    if (btns.length > 0) {
        btns[0].click();
        btns[0].click();
    }
}