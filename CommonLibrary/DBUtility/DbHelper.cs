using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;

namespace CommonLibrary.DBUtility
{
    public class DbHelper : IDbHelper
    {
        private string _ConnectionString;
        /// <summary>
        /// init DbHelper
        /// 預設DB連線字串=>設定檔section:DATABASE
        /// </summary>
        public DbHelper()
        {
            _ConnectionString = GetConnectionString("DATABASE", IniHelper.GetIniFilePath());
        }
        /// <summary>
        /// init DbHelper
        /// </summary>
        /// <param name="connectionString">自訂DB連線字串</param>
        public DbHelper(string connectionString)
        {
            _ConnectionString = connectionString;
        }
        /// <summary>
        /// 執行sql command
        /// </summary>
        /// <param name="SQLString">sql字串</param>
        /// <param name="SqlParams">參數</param>
        /// <returns>影響筆數</returns>
        public int ExecuteSql(string SQLString, List<IDbDataParameter> SqlParams)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 查詢sql command
        /// </summary>
        /// <param name="SqlString">sql字串</param>
        /// <param name="SqlParams">參數</param>
        /// <returns> 回傳List{Dictionary(string,object)} </returns>
        public IList QuerySql(string SqlString, List<IDbDataParameter> SqlParams)
        {
            return QuerySql(SqlString, SqlParams, null);
        }
        /// <summary>
        /// 查詢sql command
        /// </summary>
        /// <param name="SqlString">sql字串</param>
        /// <param name="SqlParams">參數</param>
        /// <param name="type">資料模型</param>
        /// <returns> List<資料模型> </returns>
        public IList QuerySql(string SqlString, List<IDbDataParameter> SqlParams,Type type)
        {
            IList data = new List<dynamic>();
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                        LogHelper.Write(string.Format("Open Connection :{0}", _ConnectionString));
                    }
                   
                    SqlCommand cmd = new SqlCommand(SqlString, connection);
                    if (SqlParams != null && SqlParams.Count() > 0)
                    {
                        foreach (SqlParameter para in SqlParams)
                        {
                            //if (para.SqlDbType == SqlDbType.NVarChar)
                            //{
                            //    para.Value = HttpContext.Current.Server.HtmlDecode(para.Value.ToString());
                            //}
                            cmd.Parameters.Add(para);
                        }
                    }

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dynamic model;
                        while ((dr.Read()))
                        {
                            if (type != null)
                            {
                                //自訂類別
                                model = Activator.CreateInstance(type);
                                System.Reflection.PropertyInfo[] Propertys = type.GetProperties();
                                foreach (System.Reflection.PropertyInfo pi in Propertys)
                                {
                                    //檢視db是否該欄位
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        if (string.Compare(dr.GetName(i),pi.Name,true)==0)
                                            pi.SetValue(model, dr[pi.Name]);
                                    }
                                }
                            }
                            else
                            {
                                model = new Dictionary<string, object>();
                                for (int i = 0; i < dr.FieldCount; i++)
                                    model[dr.GetName(i)] = dr[i];
                            }

                            data.Add(model);
                        }
                    }
                    return data;

                }
                catch (Exception ex)
                {
                    LogHelper.Write("[DbHelper] QuerySql error!!");
                    LogHelper.Write(ex.Message);
                    throw new Exception(ex.Message);
                }

            }
        }


        /// <summary>
        /// 取得連線字串
        /// </summary>
        /// <param name="section">設定檔section</param>
        /// <param name="iniPath">設定檔位置</param>
        /// <returns></returns>
        public static string GetConnectionString(string section, string iniPath)
        {
            try
            {
                //IniHelper.IniType type = (string.Compare(Path.GetExtension(iniPath), ".xml", true) == 0) ? IniHelper.IniType.xml : IniHelper.IniType.ini;
                return @"Data Source =" + IniHelper.GetIniFileValue(section, "servername")
                         + "; Initial Catalog = " + IniHelper.GetIniFileValue(section, "datasource")
                         + "; Integrated Security = false;User ID = " + IniHelper.GetIniFileValue(section, "userid")
                         + "; Password = '" + IniHelper.GetIniFileValue(section, "Password") + "';";
            }
            catch (Exception ex)
            {
                LogHelper.Write("[DbHelper] GetConnectionString error!!");
                throw new Exception(ex.Message);
            }
        }

    }
}
