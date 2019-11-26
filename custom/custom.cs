using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;

namespace custom
{
    
    public class custom
    {
        //public static string logString = "D:\\webApplication\\YunShiTang_JiangMenHaiGuan\\custom\\Logs";
        //public static string logString = "D:\\custom\\Logs";
        //public static string logString = "D:\\新建文件夹\\custom_new\\custom\\custom\\Logs";
        public static string logString = "D:\\custom\\custom\\Logs";
        public static object Datetime { get; private set; }

        #region start说明：这一整块是添加日志的方法

        /// <summary>
        /// 记录日志至文本文件
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="message"></param>
        public static void Log(string Path, string message)
        {

            //新建文件夹
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            Random rd = new Random();
            string FILE_NAME = Path + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + new Random().Next(1000, 9999) + ".txt";
            if (!File.Exists(FILE_NAME))
            {
                StreamWriter sr = File.CreateText(FILE_NAME);
                sr.Close();
            }

            using (StreamWriter sr = File.AppendText(FILE_NAME))
            {
                lock (sr)
                {
                    sr.WriteLine(message);
                    sr.Close();
                }
            }
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="documentName">文件名称</param>
        /// <param name="msg">要写入日志的内容</param>
        public static void WriteLog(string documentName, string msg)
        {
            string errorLogFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            if (!System.IO.Directory.Exists(errorLogFilePath))
            {
                System.IO.Directory.CreateDirectory(errorLogFilePath);
            }
            string logFile = System.IO.Path.Combine(errorLogFilePath, documentName + "@" + DateTime.Today.ToString("yyyy-MM-dd") + "qqq.txt");
            bool writeBaseInfo = System.IO.File.Exists(logFile);
            StreamWriter swLogFile = new StreamWriter(logFile, true, Encoding.Unicode);
            swLogFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t" + msg);
            swLogFile.Close();
            swLogFile.Dispose();
        }

        #endregion

        #region  start说明：这一整块是将获取的Json数据转换为DataTable的方法
        /// <summary>
        /// 将获取的Json数据转换为DataTable
        /// </summary>
        /// <param name="strJson">Json字符串</param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));
            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');
                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0].Replace("，", ",").Replace("：", ":").Replace("\"", "");
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }
        #endregion


        #region  start说明：这一整块是获取菜单类目的方法

        /// <summary>
        ///  获取菜单类目
        /// </summary>
        /// <param name="menu_type">商品种类</param>
        /// <returns></returns>
        public static string selectMenuName(string menu_type)
        {
            string result = "";
            JObject resjobj = new JObject();
            try
            {

                //string sql = "select GROUP_CONCAT(distinct(goods_type))goods_type  from tab_mall_info ";
                string sql = "select distinct(goods_type) from tab_mall_info ";
                DataTable dt = MySqlHelper.MySqlToDt(sql);


                if (dt.Rows.Count == 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("暂无菜单类目!"));
                    resjobj.Add("data", JToken.FromObject(""));
                }
                else if (dt.Rows.Count != 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("成功查询!"));
                    resjobj.Add("data", JToken.FromObject(dt));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sql));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\获取菜单类目\\" + DateTime.Now.ToString("yyyyMMdd"), sql);
                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\获取菜单类目\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        #region  start说明：这一整块是获取菜单的方法

        /// <summary>
        ///  获取菜单
        /// </summary>
        /// <param name="goods_type">商品种类</param>
        /// <returns></returns>
        public static string selectMenu(string goods_type)
        {
            string result = "";
            string sell = "";
            JObject resjobj = new JObject();
            try
            {

                string sql = "select * from tab_mall_info where goods_type like '%" + goods_type + "%' ";
                DataTable dt = MySqlHelper.MySqlToDt(sql);


                if (dt.Rows.Count == 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("暂无此类菜单!"));
                    resjobj.Add("data", JToken.FromObject(""));
                }
                else if (dt.Rows.Count != 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("成功查询!"));
                    resjobj.Add("data", JToken.FromObject(dt));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sql));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\获取菜单\\" + DateTime.Now.ToString("yyyyMMdd"), sql);
                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\获取菜单\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion


        #region  start说明：这一整块是添加订单的方法(前往下单)

        /// <summary>
        ///  添加订单
        /// </summary>
        ///// <param name="postID">商品流水号</param>
        ///// <param name="quantity">数量</param>
        //// <param name="order_id">订单编号</param>
        //// <param name="record">数组（商品流水号、数量）</param>
        /// <returns></returns>
        public static string addOrder(string order_id, string record,string phone)
        {
            string result = "";
            JObject resjobj = new JObject();

            try
            {
                if (order_id == "" || order_id == null)
                {
                    order_id = System.Guid.NewGuid().ToString();
                }

                DataTable dt = JsonToDataTable("[" + record + "]");

                    double single_sum = 0;
                    string postID = "";
                    double quantity = 0;
                    double price = 0;
                    string sql_result = "";
                    string usr_name = "";
                    string dept_name = "";
                    string goods_type = "";
                    string unit = "";
                    string goods_name = "";
                    double goods_total = 0;
                    double sell = 0;
                    double remain = 0;
               

                if (phone == "" || phone == null)
                {
                    resjobj.Add("msg", JToken.FromObject("前台传入手机号码为空！"));
                    result = JsonConvert.SerializeObject(resjobj);
                    return result;

                }
                else
                {
                    string sqlCmd2 = "select t1.phone_no,t1.usr_name,t2.Dept_Name from tab_user_info t1 join tbdeptinfo t2 on t1.dept_ID = t2.Dept_ID  where phone_no='" + phone + "'";
                    DataTable sqlCmd_result2 = MySqlHelper.MySqlToDt(sqlCmd2);
                    if (sqlCmd_result2.Rows.Count == 0)
                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("企业通讯录无录入此号码，无权预定！"));
                        resjobj.Add("data", JToken.FromObject(""));
                        result = JsonConvert.SerializeObject(resjobj);
                        return result;

                    }
                    else
                    {
                        usr_name = Convert.ToString(sqlCmd_result2.Rows[0]["usr_name"]);
                        dept_name = Convert.ToString(sqlCmd_result2.Rows[0]["Dept_Name"]);


                        for (int j = 0; j < dt.Rows.Count;)
                        {
                            postID = Convert.ToString(dt.Rows[j]["postID"]);
                            quantity = Convert.ToDouble(dt.Rows[j]["quantity"]);

                            string sqlCmd3 = "select quantity from food_order where state='未提货'and postID='" + postID + "'";
                            DataTable sqlCmd_result3 = MySqlHelper.MySqlToDt(sqlCmd3);
                            if (sqlCmd_result3.Rows.Count != 0)
                            {
                                   sell = Convert.ToDouble(sqlCmd_result3.Rows[0]["quantity"]);
                            }
                                

                            string sqlCmd = "select goods_total,goods_price,goods_type,goods_name,unit from tab_mall_info where postID='" + postID + "'";
                            DataTable sqlCmd_result = MySqlHelper.MySqlToDt(sqlCmd);

                            price = Convert.ToDouble(sqlCmd_result.Rows[0]["goods_price"]);
                            goods_type = Convert.ToString(sqlCmd_result.Rows[0]["goods_type"]);
                            unit = Convert.ToString(sqlCmd_result.Rows[0]["unit"]);
                            goods_name = Convert.ToString(sqlCmd_result.Rows[0]["goods_name"]);
                            goods_total = Convert.ToDouble(sqlCmd_result.Rows[0]["goods_total"]);
                            remain = goods_total - sell;
                            if (quantity <= remain)
                            {
                                single_sum = price * quantity;

                                string sql = "insert into food_order (phone_no, order_id, postID, quantity,money,state,usr_name,Dept_Name,goods_type,unit,goods_name,update_date ) values ('" + phone + "','" + order_id + "','" + postID + "','" + quantity + "','" + single_sum + "','待确认','" + usr_name + "','" + dept_name + "','" + goods_type + "','" + unit + "','" + goods_name + "','" + DateTime.Now.ToString() + "')";
                                sql_result = MySqlHelper.MySqlToStr(sql);
                            }
                            else
                            {

                                //return "库存不足，无法下单";
                                resjobj.Add("msg", JToken.FromObject(goods_name+"可下单库存不足，无法预定"));
                                result = JsonConvert.SerializeObject(resjobj);
                                return result;
                            }

                            if (sql_result.Contains("操作成功"))
                            {
                                j++;
                            }


                        }
                    }
                    if (sql_result.Contains("操作成功"))
                    {

                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("录入成功"));
                        resjobj.Add("data", JToken.FromObject(order_id));
                    }
                    else
                    {
                        resjobj.Add("code", JToken.FromObject("203"));
                        resjobj.Add("msg", JToken.FromObject(sql_result));
                        resjobj.Add("data", JToken.FromObject(""));
                        Log(logString + "\\前往下单\\" + DateTime.Now.ToString("yyyyMMdd"), sql_result);
                    }

                }

            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\前往下单\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        #region  start说明：这一整块是查看订单清单的方法

        /// <summary>
        ///  查看订单
        /// </summary>
        /// <param name="order_id">订单编号</param>
        /// <returns></returns>
        public static string selectOrder(string order_id)
        {
            string result = "";
            JObject resjobj = new JObject();
            try
            {

                string sql = "select goods_name,quantity,money,phone_no,usr_name from food_order  where order_id='" + order_id.Trim() + "'";
                string sql2 = "select get_goods_place from mall_setting ";
                DataTable dt = MySqlHelper.MySqlToDt(sql);
                DataTable dt2 = MySqlHelper.MySqlToDt(sql2);
        
                string get_goods_place = Convert.ToString(dt2.Rows[0]["get_goods_place"]);

                if (dt.Rows.Count == 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("暂无待确认的订单!"));
                    resjobj.Add("data", JToken.FromObject(""));
                }
                else if (dt.Rows.Count != 0 )
                
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("成功查询!"));
                    resjobj.Add("data", JToken.FromObject(dt));
                    resjobj.Add("data1", JToken.FromObject(get_goods_place));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sql));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\查看订单清单\\" + DateTime.Now.ToString("yyyyMMdd"), sql);
                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\查看订单清单\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        #region  start说明：这一整块是确认下单的方法

        /// <summary>
        ///  确认下单
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <returns></returns>
        public static string confirmOrder(string order_id)
        {
            string result = "";
            JObject resjobj = new JObject();
            try
            {


                string sqlCmd = "update food_order set state='未提货 ',order_date='" + DateTime.Now.ToString() + "' where order_id='" + order_id.Trim() + "'";
                string sqlCmd_result = MySqlHelper.MySqlToStr(sqlCmd);


                 if (sqlCmd_result.Contains("操作成功"))

                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("确认预定成功!"));
                    resjobj.Add("data", JToken.FromObject(""));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sqlCmd_result));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\确认预定\\" + DateTime.Now.ToString("yyyyMMdd"), sqlCmd_result);
                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\确认预定\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion


        #region  start说明：这一整块是查看我的订单的方法

        /// <summary>
        ///  查看我的订单
        /// </summary>
        /// <param name="state">订单编号</param>
        /// <returns></returns>
        public static string selectMyOrder(string state,string phone)
        {
            string result = "";
            string where = "";
            string where2 = "";
            string where3 = "";
            JObject resjobj = new JObject();
            try
            {
                if (state == "已完成")
                {
                    where += " where state ='已提货'and phone_no='"+phone+"'";
                }
 
               else if (state == "待取货")
                {
                    where3 += " where order_id not in ( select order_id from tab_mall_tran where state='已提货') and  state ='未提货'and phone_no='" + phone + "' ";
                }
               else if (state == "已取消")
                {
                    where2 += " where state ='已取消'and phone_no='" + phone + "'";
                }


                if (where != "")
                {
                    string sql = "select * FROM tab_mall_tran t1 join tab_mall_info t2 on t1.postId = t2.postId " + where;
                    DataTable dt = MySqlHelper.MySqlToDt(sql);
                    if (dt.Rows.Count != 0)

                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        resjobj.Add("data", JToken.FromObject(dt));
                    }
                    else
                    {
                        resjobj.Add("code", JToken.FromObject("203"));
                        resjobj.Add("msg", JToken.FromObject("没有已完成的订单！"));
                        resjobj.Add("data", JToken.FromObject(""));


                    }
                

            }

                else if (where2 != "")
                {
                    string sql2 = "select * FROM food_order t1 join tab_mall_info t2 on t1.postId = t2.postId" + where2;
                    DataTable dt2 = MySqlHelper.MySqlToDt(sql2);
                    if (dt2.Rows.Count != 0)

                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        resjobj.Add("data", JToken.FromObject(dt2));
                    }

                    else
                    {
                        resjobj.Add("code", JToken.FromObject("203"));
                        resjobj.Add("msg", JToken.FromObject("没有已取消的订单！"));
                        resjobj.Add("data", JToken.FromObject(""));


                    }
                }

                else if (where3 != "")
                {
                    string sql2 = "select * from food_order t1 join tab_mall_info t2 on t1.postId=t2.postId" + where3;
                    DataTable dt2 = MySqlHelper.MySqlToDt(sql2);
                    if (dt2.Rows.Count != 0)

                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        resjobj.Add("data", JToken.FromObject(dt2));
                    }

                    else
                    {
                        resjobj.Add("code", JToken.FromObject("203"));
                        resjobj.Add("msg", JToken.FromObject("没有待取货的订单！"));
                        resjobj.Add("data", JToken.FromObject(""));
                        //Log(logString + "\\查看我的订单\\" + DateTime.Now.ToString("yyyyMMdd"), sql2);

                    }
                }
                //string sql = "select * FROM food_order t1 join tab_mall_info t2 on t1.goods_name = t2.goods_name WHERE state = '" + state + "'";
                //DataTable dt = MySqlHelper.MySqlToDt(sql);



            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\查看我的订单\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        #region  start说明：这一整块是取消预约的方法

        /// <summary>
        ///  取消预约
        /// </summary>
        /// <param name="order_id">订单编号</param>
        /// <returns></returns>
        public static string cancelOrder(string order_id)
        {
            string result = "";
            JObject resjobj = new JObject();
            try
            {


                string sqlCmd = "update food_order set state='已取消 ',update_date='" + DateTime.Now.ToString() + "' where order_id='" + order_id.Trim() + "'";
                string sqlCmd_result = MySqlHelper.MySqlToStr(sqlCmd);


                if (sqlCmd_result.Contains("操作成功"))

                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("取消订单成功!"));
                    resjobj.Add("data", JToken.FromObject(""));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sqlCmd_result));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\取消预约\\" + DateTime.Now.ToString("yyyyMMdd"), sqlCmd_result);
                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\取消预约\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        #region  start说明：这一整块是查询二维码有效时间的方法

        /// <summary>
        ///  查询二维码有效时间
        /// </summary>
        /// <param name="order_id">订单编号</param>
        /// <returns></returns>
        public static string selectTime(string order_id)
        {
            string where = "";
            string result = "";
            JObject resjobj = new JObject();
            try
            {
                where += " where state ='未提货'and order_id = '" + order_id + "' "; 
                string sql = "select t1.goods_name,t1.order_date,t2.getgoods_end from food_order t1 join tab_mall_info t2 on t1.postId=t2.postId" + where;

                DataTable dt = MySqlHelper.MySqlToDt(sql);


                if (dt.Rows.Count != 0)

                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("成功查询!"));
                    resjobj.Add("data", JToken.FromObject(dt));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sql));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\查询postId\\" + DateTime.Now.ToString("yyyyMMdd"), sql);
                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\查询postId\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion


        //人员查询
        #region  start说明：这一整块是根据状态获得订单总数的方法

        /// <summary>
        ///  这一整块是根据状态获得订单总数的方法
        /// </summary>
        /// <param name="state">订单状态</param>
        /// <returns></returns>
        public static string selectOrderSum(string state, string date)
        {
            string where = "";
            string where2 = "";
            string where3 = "";
            string result = "";
            JObject resjobj = new JObject();
            try
            {
                if (state == "已完成")
                {
                    where += " where state ='已提货'and tran_date like  '%" + date + "%'";
                }

                else if (state == "待取货")
                {
                    
                    where3 = " where order_id not in ( select order_id  from tab_mall_tran where state='已提货') and state='未提货'  and  order_date like  '%" + date + "%'  ";
                }
                else if (state == "已取消")
                {
                    where2 = " where state ='已取消' and update_date like  '%" + date + "%' ";
                }


                if (where != "")
                {
                    string sql = "select count(distinct order_id )as orderSum FROM tab_mall_tran " + where;
                    DataTable dt = MySqlHelper.MySqlToDt(sql);
                    if (dt.Rows.Count != 0)

                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        resjobj.Add("data", JToken.FromObject(dt));
                    }
                    else
                    {
                        resjobj.Add("code", JToken.FromObject("203"));
                        resjobj.Add("msg", JToken.FromObject("暂无已完成订单"));
                        resjobj.Add("data", JToken.FromObject(""));


                    }


                }



                if (where2 != "")
                {
                    string sql2 = "select count(distinct order_id )as orderSum FROM food_order " + where2;
                    DataTable dt2 = MySqlHelper.MySqlToDt(sql2);
                    if (dt2.Rows.Count != 0)

                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        resjobj.Add("data", JToken.FromObject(dt2));
                    }
                    else
                    {
                        resjobj.Add("code", JToken.FromObject("203"));
                        resjobj.Add("msg", JToken.FromObject("暂无已取消订单"));
                        resjobj.Add("data", JToken.FromObject(""));


                    }

                   

                }

                if (where3 != "")
                {
                    string sql2 = "select count(distinct order_id )as orderSum FROM food_order " + where3;
                    DataTable dt2 = MySqlHelper.MySqlToDt(sql2);
                    if (dt2.Rows.Count != 0)

                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        resjobj.Add("data", JToken.FromObject(dt2));
                    }
                    else
                    {
                        resjobj.Add("code", JToken.FromObject("203"));
                        resjobj.Add("msg", JToken.FromObject("暂无待提货订单"));
                        resjobj.Add("data", JToken.FromObject(""));


                    }

                   

                }



            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\根据状态获得订单总数\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        #region  start说明：这一整块是根据状态获得取货查询信息（人员查询）的方法

        /// <summary>
        ///  根据状态获得取货查询信息（人员查询）
        /// </summary>
        /// <param name="state">订单状态</param>
        /// <returns></returns>
        public static string selectState(string state, string date)
        {
            string where = "";
            string where2 = "";
            string where3 = "";
            string result = "";
            JObject resjobj = new JObject();
            try
            {

                if (state == "已完成")
                {
                    where += "  where state ='已提货'and tran_date like  '%" + date + "%' ";
                }
                else if (state == "待取货")
                {
                    where3 = " where order_id not in ( select order_id from tab_mall_tran where state='已提货') and state='未提货' and order_date like  '%" + date + "%'  ";
                }
                else if (state == "已取消")
                {
                    where2 = " where state ='已取消'and update_date like  '%" + date + "%' ";
                }



                if (where != "")
                {
                    string sql = "select usr_name,phone_no,GROUP_CONCAT(goods_name) names  from tab_mall_tran " + where + " group by order_id";
                    DataTable dt = MySqlHelper.MySqlToDt(sql);
                    if (dt.Rows.Count != 0)

                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        resjobj.Add("data", JToken.FromObject(dt));
                    }
                    else
                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("暂无此类订单"));
                        resjobj.Add("data", JToken.FromObject(""));

                    }


                }

                if (where2 != "")
                {
                    string sql2 = "select usr_name,phone_no,GROUP_CONCAT(goods_name) names  from food_order " + where2 + " group by order_id";
                    DataTable dt2 = MySqlHelper.MySqlToDt(sql2);
                    if (dt2.Rows.Count != 0)

                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        resjobj.Add("data", JToken.FromObject(dt2));
                    }
                    else
                    {
                        resjobj.Add("code", JToken.FromObject("200"));
                        resjobj.Add("msg", JToken.FromObject("暂无此类订单"));
                        resjobj.Add("data", JToken.FromObject(""));


                    }
                }

                    if (where3 != "")
                    {
                        string sql3 = "select usr_name,phone_no,GROUP_CONCAT(goods_name) names  from food_order " + where3 + " group by order_id";
                        DataTable dt3 = MySqlHelper.MySqlToDt(sql3);
                        if (dt3.Rows.Count != 0)

                        {
                            resjobj.Add("code", JToken.FromObject("200"));
                            resjobj.Add("msg", JToken.FromObject("成功查询!"));
                            resjobj.Add("data", JToken.FromObject(dt3));
                        }
                        else
                        {
                            resjobj.Add("code", JToken.FromObject("200"));
                            resjobj.Add("msg", JToken.FromObject("暂无此类订单"));
                            resjobj.Add("data", JToken.FromObject(""));


                        }


                    }




            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\根据状态获得取货查询信息（人员查询）\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        //商品查询
        #region  start说明：这一整块是查询取货查询信息（商品查询）

        /// <summary>
        /// 取货查询信息（商品查询）
        /// </summary>
        /// <param name="state">销售状态</param>
        /// <returns></returns>
        public static string selectGoods(string date)
        {
            string result = "";
            string where = "";
            string where2 = "";
            JObject resjobj = new JObject();
            try
            {
             
                where += " where state ='已提货' and tran_date like  '%" + date + "%' ";
                string sql2 = " select t1.goods_name,sum(quantity) quantity,t2.goods_price,t2.unit,sum(money)sum from tab_mall_tran t1 join tab_mall_info t2 on t1.postID=t2.postID" + where + " group by t1.goods_name,t1.postID  ";  
                DataTable sql2_result = MySqlHelper.MySqlToDt(sql2);

                //where2 = " where total_update like  '%" + date + "%' ";
                //string sql = "select goods_name,goods_price,unit,real_total,(goods_price * real_total)sum from tab_mall_info  " + where2 + "  GROUP BY goods_name,postID";
                string sql = "select goods_name,goods_price,unit,real_total,(goods_price * real_total)sum from tab_mall_info  where total_update like  '%" + date + "%' GROUP BY goods_name,postID  UNION select goods_name,goods_price,unit,real_total,(goods_price * real_total)sum from tab_mall_info  where total_update not like  '%" + date + "%' and real_total >0";
                DataTable dt = MySqlHelper.MySqlToDt(sql);


                if (sql2_result.Rows.Count != 0 || dt.Rows.Count != 0 )
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("成功查询!"));
                    resjobj.Add("data", JToken.FromObject(sql2_result));
                    resjobj.Add("data1", JToken.FromObject(dt));

                }


                else
                {
                    string sql3 = sql + sql2;
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject("暂无数据！"));
                    resjobj.Add("data", JToken.FromObject(""));

                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\取货查询信息（商品查询）\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion



        #region  start说明：这一整块是添加评论的方法

        /// <summary>
        ///  添加评论
        /// </summary>
        ///// <param name="best">喜欢/不喜欢</param>
        ///// <param name="score_look">外观分数</param>
        ///// <param name="ccore_color">菜色分数</param>
        ///// <param name="score_smell">菜香分数（菜名、数量）</param>
        ///// <param name="score_taste">菜味分数</param>
        ///// <param name="score_chef">厨师分数</param>
        //// <param name="phone">手机号码</param>
        ////<param name="name">菜名</param>
        /// <returns></returns>
        public static string addRemark(string best, string score_look, string score_color,string score_smell,string score_taste,string score_chef,string comment,string phone,string name)
        {
            string result = "";
            string sql_result = "";
            double rate = 0;
            JObject resjobj = new JObject();
            try
            {
                //rate = 
                string sql = "insert into food_remark (best,score_look,score_color,score_taste,score_chef,comment,phone_no,goods_name,input_date) values ('" + best + "','" + score_look + "','" + score_smell + "','" + score_taste + "','" + score_chef + "','" + comment + "','" + phone + "','" + name + "','" + DateTime.Now.ToString() + "')";
                sql_result = MySqlHelper.MySqlToStr(sql);

                if (sql_result.Contains("操作成功"))
                {

                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("录入成功"));
                    resjobj.Add("data", JToken.FromObject(""));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sql_result));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\添加评论\\" + DateTime.Now.ToString("yyyyMMdd"), sql_result);
                }
            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\添加评论\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        #region  start说明：这一整块是查看评论的方法

        /// <summary>
        ///  查看评论
        /// </summary>
        /// <param name="name">菜名</param>
        /// <returns></returns>
        public static string selectRemark(string name)
        {
            string result = "";
            JObject resjobj = new JObject();
            try
            {

                string sql = "select t1.best,t1.comment,t1.usr_name,t1.goods_name,t2.chef from food_remark t1 join food_info t2 on t1.goods_name = t2.name  where goods_name='" + name.Trim() + "'";
                DataTable dt = MySqlHelper.MySqlToDt(sql);

                if (dt.Rows.Count == 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("暂无评论!"));
                    resjobj.Add("data", JToken.FromObject(""));
                }
                else if (dt.Rows.Count != 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("成功查询!"));
                    resjobj.Add("data", JToken.FromObject(dt));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sql));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\查看评论\\" + DateTime.Now.ToString("yyyyMMdd"), sql);
                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\查看评论\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion

        #region  start说明：这一整块是查看菜品信息的方法

        /// <summary>
        ///  查看菜品信息
        /// </summary>
        /// <param name="name">菜名</param>
        /// <returns></returns>
        public static string select_info(string name)
        {
            string result = "";
            JObject resjobj = new JObject();
            try
            {

                string sql = "select t1.name,t1.chef,t2.PicPath from food_info t1 join tab_mall_info on t1.name = t2.goods.name  where goods_name='" + name.Trim() + "'";
                DataTable dt = MySqlHelper.MySqlToDt(sql);

                if (dt.Rows.Count == 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("暂无评论!"));
                    resjobj.Add("data", JToken.FromObject(""));
                }
                else if (dt.Rows.Count != 0)
                {
                    resjobj.Add("code", JToken.FromObject("200"));
                    resjobj.Add("msg", JToken.FromObject("成功查询!"));
                    resjobj.Add("data", JToken.FromObject(dt));
                }
                else
                {
                    resjobj.Add("code", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject(sql));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\查看评论\\" + DateTime.Now.ToString("yyyyMMdd"), sql);
                }


            }
            catch (Exception ex)
            {
                resjobj.Add("code", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\查看评论\\" + DateTime.Now.ToString("yyyyMMdd"), ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;
        }

        #endregion



        #region  start说明：这一整块是获取手机号码的方法

        /// <summary>
        /// 获取手机号码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>


        public /*static*/ string getData(string code)
        {
            string result = "";
            string phone = "";
            //string codes = code;
            JObject resjobj = new JObject();
            JObject jobj = new JObject();
            try
            {


                //code = jobject(jobj, "code");

                if (code != "null")
                {
                    //Log(logString + "\\传进getDate的code值\\" + DateTime.Now.ToString("yyyyMMdd"), "code:" + code);
                    phone = getUserPhone(code);
                    //Log(logString + "\\getData\\" + DateTime.Now.ToString("yyyyMMdd"), "phone:"+phone);

                    //从通讯录获取手机号码
                    //string phone = "13686948977";
                    //判断手机号码是否为空
                    if (phone != "")
                    {



                        string sql = "select phone_no FROM tab_user_info WHERE phone_no = '" + phone + "'";
                        //Log(logString + "\\getData\\" + DateTime.Now.ToString("yyyyMMdd"), "sql语句已执行"+sql);

                        DataTable dt = MySqlHelper.MySqlToDt(sql);
                        //Log(logString + "\\getData\\" + DateTime.Now.ToString("yyyyMMdd"), Convert.ToString("dt的行数"+ dt.Rows.Count));


                        if (dt.Rows.Count == 0)
                        {
                        
                            resjobj.Add("codes", JToken.FromObject("200"));
                            resjobj.Add("msg", JToken.FromObject("暂无此人信息!"));
                            resjobj.Add("data", JToken.FromObject(""));
                        }
                        else if (dt.Rows.Count != 0)
                        {
                            //Log(logString + "\\getData\\" + DateTime.Now.ToString("yyyyMMdd"), sql);
                            resjobj.Add("codes", JToken.FromObject("200"));
                            resjobj.Add("msg", JToken.FromObject("成功查询!"));
                            resjobj.Add("data", JToken.FromObject(phone));

                        }


                        //resjobj.Add("code", JToken.FromObject(200));
                        //resjobj.Add("msg", JToken.FromObject("成功查询!"));
                        //resjobj.Add("data", JToken.FromObject(phone));


                    }
                    else
                    {
                        resjobj.Add("codes", JToken.FromObject("203"));
                        resjobj.Add("msg", JToken.FromObject("无法获取手机号码"));
                        resjobj.Add("data", JToken.FromObject(""));
                        Log(logString + "\\获取手机号码异常\\" + DateTime.Now.ToString("yyyyMMdd"), "无法获取手机号码");
                        //result = "{\"result\":\"333\",\"msg\":\"无法获取手机号码\"}";
                    }
                }
                else
                {
                    resjobj.Add("codes", JToken.FromObject("203"));
                    resjobj.Add("msg", JToken.FromObject("后台code参数错误"));
                    resjobj.Add("data", JToken.FromObject(""));
                    Log(logString + "\\获取手机号码异常\\" + DateTime.Now.ToString("yyyyMMdd"), "后台code参数错误");
                    //result = "{\"result\":\"333\",\"msg\":\"后台code参数错误\"}";
                }
            }

            catch (Exception ex)
            {
                resjobj.Add("codes", JToken.FromObject("3001"));
                resjobj.Add("msg", JToken.FromObject(ex.Message.ToString()));
                resjobj.Add("data", JToken.FromObject(""));
                Log(logString + "\\获取手机号码异常\\" + DateTime.Now.ToString("yyyyMMdd"), "3001错误"+ ex.Message.ToString());
            }
            result = JsonConvert.SerializeObject(resjobj);
            return result;

            //}
        }

        #endregion

        /// <summary>
        /// 调用access_token和userid获得用户手机号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getUserPhone(string code)
        {
            try
            {
                //获取access_token
                string access_token = getAccessToken();
                //获取userid
                if (access_token != "")
                {
                    string userid = getUserId(code, access_token);
                    //Log(logString + "\\getUserPhone\\" + DateTime.Now.ToString("yyyyMMdd"), "userid:"+userid);

                    if (userid != "")
                    {
                        //获取phone
                        string phone = getMobile(userid, access_token);
                        //Log(logString + "\\getUserPhone\\" + DateTime.Now.ToString("yyyyMMdd"), "phone:"+phone);
                        return phone;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                string e = ex.Message.ToString();
                //LogClass.WriteLogFile("TemCard\\error", "调用接口时异常：" + ex.Message.ToString());
                Log(logString + "\\getUserPhone函数异常\\" + DateTime.Now.ToString("yyyyMMdd"), "调用接口时异常(3001)：" + ex.Message.ToString());

                return "";
            }

        }

        /// <summary>
        /// 调用微信接口获access_token
        /// </summary>
        /// <returns></returns>
        public string getAccessToken()
        {
            string corpid = System.Configuration.ConfigurationManager.AppSettings["CorpidStr"].ToString();
            string corpsecret = System.Configuration.ConfigurationManager.AppSettings["CorpsecretStr"].ToString();
            try
            {
                SendHttp acc_con = new SendHttp();
                //string accessStr = acc_con.getMsg("GET", "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=wxe6c505f9867d394c&corpsecret=8Plzvkxjk5AwbT9TgxQtA-Hm1fxMPM0aqu2fnERNY671yh4Flot2tdxWvPKJ8o-2");
                string accessStr = acc_con.getMsg("GET", "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret);
                //Log(logString + "\\getAccessToken\\" + DateTime.Now.ToString("yyyyMMdd"), "accessStr:" + accessStr);

                if (string.IsNullOrEmpty(accessStr))
                {
                    return "";
                }
                else
                {
                    JObject accObj = JObject.Parse(accessStr);
                    string access_token = jobject(accObj, "access_token");
                    //LogClass.WriteLogFile("TemCard\\info", ident + "access_token" + access_token);
                    //Log(logString + "\\getAccessToken\\" + DateTime.Now.ToString("yyyyMMdd"), "access_token:"+access_token);

                    return access_token;
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message.ToString();
                //LogClass.WriteLogFile("TemCard\\error", ident + "access_token异常：" + e);
                Log(logString + "\\getUserPhone函数异常\\" + DateTime.Now.ToString("yyyyMMdd"), "access_token异常：" + e);

                return "";
            }
        }
        //获取url中的利用code获取userid
        public string getUserId(string code, string access_token)
        {
            try
            {
                SendHttp user_con = new SendHttp();
                string userStr = user_con.getMsg("GET", "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token=" + access_token + "&code=" + code + "");
                //LogClass.WriteLogFile("TemCard\\info", ident + "userStr" + userStr);
                //Log(logString + "\\getUserId\\" + DateTime.Now.ToString("yyyyMMdd"), "userStr:" + userStr);

                if (userStr.IndexOf("UserId") > -1)
                {
                    JObject user = JObject.Parse(userStr);
                    string userId = jobject(user, "UserId");
                    //LogClass.WriteLogFile("TemCard\\info", ident + "userId" + userId);
                //Log(logString + "\\getUserId\\" + DateTime.Now.ToString("yyyyMMdd"), "userId:" + userId);
                    return userId;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                string e = ex.Message.ToString();
                //LogClass.WriteLogFile("TemCard\\error", "UserId异常：" + e);
                Log(logString + "\\getUserId函数异常\\" + DateTime.Now.ToString("yyyyMMdd"), "UserId异常：" + e);

                return "";
            }
        }

        //通过userid和access_token获取成员信息
        public string getMobile(string userid, string access_token)
        {
            try
            {
                SendHttp staff_con = new SendHttp();
                string msgStr = staff_con.getMsg("GET", "https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token=" + access_token + "&userid=" + userid + "");
                //LogClass.WriteLogFile("TemCard\\info", ident + "msgStr" + msgStr);
                //Log(logString + "\\getMobile\\" + DateTime.Now.ToString("yyyyMMdd"),  "msgStr:" + msgStr);
                if (msgStr.IndexOf("mobile") > -1)
                {
                    JObject staff = JObject.Parse(msgStr);
                    string mobile = jobject(staff, "mobile");
                    //LogClass.WriteLogFile("TemCard\\info", ident + "mobile" + mobile);
                    //Log(logString + "\\getMobile\\" + DateTime.Now.ToString("yyyyMMdd"), "mobile:" + mobile);

                    return mobile;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                string e = ex.Message.ToString();
                //LogClass.WriteLogFile("TemCard\\error", ident + "mobile异常：" + e);
                Log(logString + "\\getMobile函数异常\\" + DateTime.Now.ToString("yyyyMMdd"), "mobile异常：" + e);

                return "mobile";
            }
        }

        /// 从jobject中获取相应的数据 
        /// </summary>
        /// <param name="jobj">jobject对象</param>
        /// <param name="key">要获取的值</param>
        /// <returns></returns>
        public string jobject(JObject jobj, string key)
        {
            string hh = "";
            if (jobj[key] != null)
            {
                hh = jobj[key].ToString().Trim();
            }
            return hh;
        }




    } 
 }

