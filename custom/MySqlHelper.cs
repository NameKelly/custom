using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;


namespace custom
{
    public class MySqlHelper
    {
        public static string myConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /// <summary>
        /// 查询mysql数据库到DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable MySqlToDt(string sql)
        {
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            myConnection.Open();
            MySqlDataAdapter Mda = new MySqlDataAdapter(sql, myConnection);
            DataTable Dt = new DataTable();
            Mda.Fill(Dt);
            myConnection.Close();
            return Dt;
        }


        /// <summary>
        /// 插入数据到mysql数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string MySqlToStr(string sql)
        {
            MySqlConnection mysql = new MySqlConnection(myConnectionString);
            mysql.Open();
            MySqlCommand mysqlcommand = new MySqlCommand(sql, mysql);
            string result = "";
            try
            {
                int iRet = mysqlcommand.ExecuteNonQuery();
                if (iRet > 0)
                {
                    result = "操作成功";
                }
                else
                {
                    result = "受影响行数为0";
                }
            }
            catch (MySqlException ex)
            {
                string message = ex.Message;
                result = "操作失败；" + message;
            }
            finally
            {
                mysql.Close();
            }
            return result;
        }



    }
}