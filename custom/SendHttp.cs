using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace custom
{
    public class SendHttp
    {
        public String getMsg(string method, string url)
        {

            string strResult = "";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.Method = method;
                myRequest.ReadWriteTimeout = 20000;
                myRequest.Timeout = 20000;
                myRequest.ServicePoint.Expect100Continue = false;
                System.Net.ServicePointManager.DefaultConnectionLimit = 50;
                try
                {
                    myRequest.Proxy = null;
                    HttpWebResponse HttpWResp = (HttpWebResponse)myRequest.GetResponse();
                    Stream myStream = HttpWResp.GetResponseStream();
                    StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
                    StringBuilder strBuilder = new StringBuilder();
                    while (-1 != sr.Peek())
                    {
                        strBuilder.Append(sr.ReadLine());
                    }
                    strResult = strBuilder.ToString();
                    HttpWResp.Close();
                    sr.Close();
                    return strResult;
                }
                catch (Exception ex)
                {
                    string e = ex.Message.ToString();
                    //LogClass.WriteLogFile("TemCard\\error", "发送请求时发生的错误second:" + e);
                    return "";
                }
                finally
                {
                    myRequest.Abort();
                }

            }
            catch (Exception ex)
            {
                string e = ex.Message.ToString();
                //LogClass.WriteLogFile("TemCard\\error", "发送请求时发生的错误first:" + e);
                return "";

            }
        }
    }
}