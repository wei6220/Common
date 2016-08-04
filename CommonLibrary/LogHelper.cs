using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CommonLibrary
{
    /// <summary>
    /// log讀取元件
    /// </summary>
    public class LogHelper
    {
        private static string _LogDirPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Log";
        private static string _LogFileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
        private static object _lockWrite = new object();
        /// <summary>
        /// 寫入log
        /// </summary>
        /// <param name="logStr">寫入字串</param>
        public static void Write(string logStr)
        {
            Write(logStr, _LogFileName);
        }
        /// <summary>
        /// 寫入log
        /// </summary>
        /// <param name="logStr">寫入字串</param>
        /// <param name="fileName">檔案名稱</param>
        public static void Write(string logStr, string fileName)
        {
            Write(logStr, fileName, _LogDirPath);
        }
        /// <summary>
        /// 寫入log
        /// </summary>
        /// <param name="logStr">寫入字串</param>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="dirPath">log目錄位置</param>
        public static void Write(string logStr, string fileName,string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                StreamWriter sw;
                lock (_lockWrite)
                {
                    if (!File.Exists(dirPath + @"\" + fileName))
                        sw = File.CreateText(dirPath + @"\" + fileName);
                    else
                        sw = new StreamWriter(dirPath + @"\" + fileName, true);
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " || " + logStr);
                    sw.Close();
                }
            }
            catch (Exception ex) {
                Write(ex.Message);
            }
        }
    }
}
