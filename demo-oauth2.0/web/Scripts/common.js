// 获取路径查询参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var reg_rewrite = new RegExp("(^|/)" + name + "/([^/]*)(/|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    var q = window.location.pathname.substr(1).match(reg_rewrite);
    if (r != null) {
        return unescape(r[2]);
    } else if (q != null) {
        return unescape(q[2]);
    } else {
        return null;
    }
}

// 测试token是否正确
function testToken(token) {
    $.ajax({
        type: 'get',
        headers: {
            'Authorization': 'bearer ' + token
        },
        url: '../../api/values',
        success: function (data) {
            console.log(data);
        }
    })
}