
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BookDinner;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace BookDinner
{
    public class GetWeiXinInfo
    {
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
                    //if (userid != "")
                    if (userid != "")
                    {
                        //获取phone
                        string phone = getMobile(userid, access_token);
                        string phone_userid = userid + "|" + phone;
                        //string phone_userid = "TanMeiDong" + "|" + "13686948977";
                        return phone_userid;
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
                string accessStr = acc_con.getMsg("GET", "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret);
                if (string.IsNullOrEmpty(accessStr))
                {
                    return "";
                }
                else
                {
                    JObject accObj = JObject.Parse(accessStr);
                    string access_token = jobject(accObj, "access_token");
                    LogClass.WriteLogFile("BookDin\\info",access_token);
                    return access_token;
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message.ToString();
                LogClass.WriteLogFile("BookDin\\error","access_token异常：" + e);
                return "";
            }

        }

        /// <summary>
        /// 用code获取userid
        /// </summary>
        /// <param name="code"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string getUserId(string code, string access_token)
        {
            try
            {
                SendHttp user_con = new SendHttp();
                string userStr = user_con.getMsg("GET", "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token=" + access_token + "&code=" + code + "");
                LogClass.WriteLogFile("BookDin\\info","userStr" + userStr);
                if (userStr.IndexOf("UserId") > -1)
                {
                    JObject user = JObject.Parse(userStr);
                    string userId = jobject(user, "UserId");
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
                LogClass.WriteLogFile("BookDin\\error","UserId异常：" + e);
                return "";
            }
        }

        /// <summary>
        /// 通过userid和access_token获取成员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string getMobile(string userid, string access_token)
        {
            try
            {
                SendHttp staff_con = new SendHttp();
                string msgStr = staff_con.getMsg("GET", "https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token=" + access_token + "&userid=" + userid + "");
                LogClass.WriteLogFile("BookDin\\info","msgStr:" + msgStr);
                if (msgStr.IndexOf("mobile") > -1)
                {
                    JObject staff = JObject.Parse(msgStr);
                    string mobile = jobject(staff, "mobile");
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
                LogClass.WriteLogFile("BookDin\\error","mobile异常：" + e);
                return "mobile";
            }
        }

        #region
        /// <summary>
        /// 这个也是转换为json不过是用系统的
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string DataTableToJson(DataTable dt)
        {
            string result = "";
            result = JsonConvert.SerializeObject(dt, new DataTableConverter());
            //new DataTableConverter()一般是固定的
            return result;
        }
        /// <summary>     
        /// dataTable转换成Json格式     
        /// </summary>     
        /// <param name="dt"></param>     
        /// <returns></returns>     
        public static string tableToJson(DataTable dt, string tablename)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("\"");
            jsonBuilder.Append(tablename);
            jsonBuilder.Append("\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower());
                        jsonBuilder.Append("\":\"");
                        if (dt.Columns[j].DataType == typeof(DateTime))
                        {
                            String v = dt.Rows[i][j].ToString().Trim();
                            if (v != "")
                            {
                                DateTime d = DateTime.Now;
                                DateTime.TryParse(v, out d);
                                v = d.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            jsonBuilder.Append(v);
                        }
                        else
                        {
                            String v = dt.Rows[i][j].ToString().Trim();
                            v = v.Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "").Replace("\\", "\\\\");
                            jsonBuilder.Append(v);
                        }
                        jsonBuilder.Append("\",");
                    }

                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
            }
            else
            {
                jsonBuilder.Append(",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);

            jsonBuilder.Append("]");
            //jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }



        /// <summary>
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

        #endregion
    }

}
