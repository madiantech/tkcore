function testHelloGetValue(elm) {
    var fieldStr = elm.Control.attr("data-fields");
    var fields = fieldStr.parseJSON();
    var result = [];
    $.each(fields, function (index, element) {
        var filter = "input[data-name=" + element.NickName + "]";
        var ctrl = elm.Control.find(filter);
        var item = { "Name": element.NickName, "Value": ctrl.val() };
        result.push(item);
    });
    return result;
}

function testHelloSetValue(data, field, value) {
    $.each(value, function (index, element) {
        data[element.Name] = element.Value;
    });
}