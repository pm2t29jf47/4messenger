using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace DataSourceLayer
{
    /// <summary> 
    ///Базовый класс для уровня доступа к данным
    /// </summary>
    public class Gateway
    {
        static Gateway()
        {
            sqlConnection.Open();
        }

        /// <summary> 
        /// Возвращает строку подключения 
        /// </summary>
        protected static string dbSource
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DB"].ConnectionString.ToString();
            }
        }
        protected static SqlConnection sqlConnection = new SqlConnection(dbSource);       
    }
}