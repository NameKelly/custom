//当前日期
function getDate() {
    var now = new Date();
    var year = now.getFullYear();//获取四位数年数
    var month = now.getMonth() + 1;
    var date = now.getDate();
    var weeknum = now.getDay();

    var nowDate = year + "-" + Appendzero(month) + "-" + Appendzero(date);
    return nowDate;
}

function Appendzero(obj) {
    if (obj < 10) return "0" + "" + obj;
    else return obj;
}

//接受地址栏参数(时间)
function urlDate() {
    var url = location.search; //获取url中"?"符后的字串 ('?modFlag=business&role=1')
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1); //substr()方法返回从参数值开始到结束的字符串；
        var strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = (strs[i].split("=")[1]);
        }
        console.log(theRequest); //此时的theRequest就是我们需要的参数； 
        return theRequest.data;
    }
}