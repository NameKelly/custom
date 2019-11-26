
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BookDinner
{
    public class LogClass
    {
        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input"></param>
        public static void WriteLogFile(string path, string log)
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + "log" + "\\" + path;
            try
            {
                if (!Directory.Exists(filename))
                {
                    Directory.CreateDirectory(filename);
                }
            }
            catch { }
            filename += "/日志" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            try
            {
                if (!File.Exists(filename))
                {
                    StreamWriter sr = File.CreateText(filename);
                    sr.Close();
                }
                using (StreamWriter sr = File.AppendText(filename))
                {
                    lock (sr)
                    {
                        sr.WriteLine(DateTime.Now.ToString() + ":" + log);
                        sr.WriteLine("\n");
                        sr.Close();
                    }
                }
            }
            catch
            {
            }
        }
    }

}