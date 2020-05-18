<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="custom.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <title>测试</title>
    <script src="http://code.jquery.com/jquery-1.8.0.min.js"></script>
    <script src="Scripts/jquery.cookie.js"type="text/javascript"></script>
    <script src="Scripts/jquery.form.js"type="text/javascript"></script>
    <script src="Scripts/json2.js"type="text/javascript"></script>
    <script>


        //$(document).ready(function () {
        //    var code = GetQueryString("code");
        //    //var code = "666886";
        //    if (code != null && code.toString().length > 1) {
        //        $.ajax({
        //            type: "post",
        //            url: "api/getData",
        //            data: { code:code },

        //            success: function (data) {
        //                alert(data);

        //            }
        //        });
        //    }
        //    else{
        //        alert("无权访问！");

        //    }
        //})

        ////获取当前地址中的参数
        //function GetQueryString(name) {
        //    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        //    var r = window.location.search.substr(1).match(reg);
        //    if (r != null) {
        //        return unescape(r[2]);
        //    }
        //    return null;
        //}


        //获取菜单类目
        function select_menuName() {

            $.ajax({
                type: "post",
                url: "api/select_menuName",
                data: { menu_type: "" },

                success: function (data) {
                    alert(data);

                }
            });
        }




        //获取菜单
        function select_menu() {

            $.ajax({
                type: "post",
                url: "api/select_menu",
                data: { goods_type: "菜" },

                success: function (data) {
                    alert(data);

                }
            });
        }



            //添加订单
        function add_order() {
            $.ajax({
                type: "post",
                url: "api/add_order",
                data: {

                    phone:"13686948977",
                    record: JSON.stringify([
                        //{ postID: "311650483", quantity: "2" },
                        //{ postID: "311650482", quantity: "1" }

                        { postID: "12313", quantity: "2" }
                   
                    ])
                },
                traditional:true,
                success: function (data) {
                    alert(data);

                }
            });
        }

            //查看订单
            function select_order() {

                $.ajax({
                    type: "post",
                    url: "api/select_order",
                    data: { order_id: "e758b672-c34a-47a8-8097-dd639ac6178e" },

                    success: function (data) {
                        alert(data);

                    }
                });
            }

            //确认下单
            function confirm_order() {

                $.ajax({
                    type: "post",
                    url: "api/confirm_order",
                    data: { order_id: "e758b672-c34a-47a8-8097-dd639ac6178e" },

                    success: function (data) {
                        alert(data);

                    }
                });
            }



            //根据状态获得取货查询信息（人员查询）
            function select_state() {

                $.ajax({
                    type: "post",
                    url: "api/select_state",
                    data: { state: "待取货", date: "2019-10-12" },

                    success: function (data) {
                        alert(data);

                    }
                });
            }

            //根据状态获得订单总数
            function select_orderSum() {

                $.ajax({
                    type: "post",
                    url: "api/select_orderSum",
                    data: { state: "已完成", date: "2019-10-12" },

                    success: function (data) {
                        alert(data);

                    }
                });
            }

            //查询取货查询信息（商品查询）
            function select_goods() {

                $.ajax({
                    type: "post",
                    url: "api/select_goods",
                    data: { date: "2019-11-08" },

                    success: function (data) {
                        alert(data);

                    }
                });
            }

            //添加评论
            function add_remark() {

                $.ajax({
                    type: "post",
                    url: "api/add_remark",
                    data: { best: "1", score_look: "6", score_color: "6", score_smell: "6", score_taste: "6", score_chef: "6", comment: "色香味俱全！", phone: "13686948977" ,name:"火腿"},

                    success: function (data) {
                        alert(data);

                    }
                });
            }

            //查看评论
            function select_remark() {

                $.ajax({
                    type: "post",
                    url: "api/select_remark",
                    data: { name: "西瓜" },

                    success: function (data) {
                        alert(data);

                    }
                });
        }

                    //取消订单
            function cancel_order() {

                $.ajax({
                    type: "post",
                    url: "api/cancel_order",
                    data: { order_id: "1c2bfe97-fec2-4ce9-8ebc-6e7ac3b90a12" },

                    success: function (data) {
                        alert(data);

                    }
                });
            }

            //查看我的订单
            function select_myOrder() {

                $.ajax({
                    type: "post",
                    url: "api/select_myOrder",
                    data: { state: "待取货",phone:"13686948977"  },

                    success: function (data) {
                        alert(data);

                    }
                });
            }

            //查看二维码有效时间
            function select_QRCode() {

                $.ajax({
                    type: "post",
                    url: "api/QRCode",
                    data: { order_id: "7c8ceabd-4374-0361-f435-76cf2e9d57d8" },

                    success: function (data) {
                        alert(data);

                    }
                });
            }

         </script>
</head>
<body>
    <form id="form1" runat="server"  method="post" enctype="multipart/form-data">
    <div>
         <input type="button" onclick="add_order()" value="添加订单" /> 
        <hr />
         <input type="button" onclick="select_order()" value="查看订单" />  
         <hr />
         <input type="button" onclick="confirm_order()" value="确认下单" />   
         <hr />
         <input type="button" onclick="select_menuName()" value="获取菜单类目" />     
        <hr />
         <input type="button" onclick="select_menu()" value="获取菜单" />    
        <hr />
         <input type="button" onclick="select_state()" value="根据状态获得取货查询信息（人员查询）" />  
         <hr />
         <input type="button" onclick="select_orderSum()" value="根据状态获得订单总数" />  
         <hr />
         <input type="button" onclick="select_goods()" value="查询取货查询信息（商品查询）" />  
         <hr />
         <input type="button" onclick="add_remark()" value="添加评论" />   
         <hr />
         <input type="button" onclick="select_remark()" value="查看评论" />
         <hr />
         <input type="button" onclick="cancel_order()" value="取消订单" />    
          <hr />
         <input type="button" onclick="select_myOrder()" value="查看我的订单" />  
          <hr />
         <input type="button" onclick="select_QRCode()" value="查看二维码有效时间" />       
                    
    </div>
    </form>
</body>
</html>
