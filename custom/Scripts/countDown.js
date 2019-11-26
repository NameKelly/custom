
/**
 * 将时间转换成时间戳
 * @param DateTime 为时间格式下的时间 2018/06/14 13:00:00或2018-06-14 13:00:00
 * @returns {number}
 * @constructor
 */

// 计算时间差
function DownTime(EndTime) {
    //如果为时间戳
    var EndTimes = new Date(EndTime).getTime();//结束时间

    var NowTime = new Date().getTime();//当前时间

    var DeltaT = EndTimes - NowTime;
    //计算出相差天数
    var days = Math.floor(DeltaT / (24 * 3600 * 1000));

    //计算出小时数

    var leave1 = DeltaT % (24 * 3600 * 1000);
    var H = Math.floor(leave1 / (3600 * 1000));
    //计算相差分钟数
    var leave2 = leave1 % (3600 * 1000);
    var M = Math.floor(leave2 / (60 * 1000));
    //计算相差秒数
    var leave3 = leave2 % (60 * 1000);
    var S = Math.round(leave3 / 1000);
    var reminder;
    if (DeltaT > 0) {
        H < 10 ? H = '0' + H : H
        M < 10 ? M = '0' + M : M
        S < 10 ? S = '0' + S : S
        if (days != "") {          
            reminder = days + "天 " + H + ":" + M + ":" + S + "";
        } else if (days == "" || H != "") {
            reminder = H + ":" + M + ":" + S + "";
        }
    } else {
        //reminder = "请注意！时间到了！";
        reminder = "00:00:00";
    }
    return reminder;

}
