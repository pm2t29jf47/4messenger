using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace DataSourceLayer
{
   public class Gateway
    {
       /// <summary>
       /// Пул подключений
       /// </summary>
       private static Dictionary<string, SqlConnection> customConnectionPool = new Dictionary<string, SqlConnection>();

       /// <summary>
       /// Возвращает подключение из пула
       /// </summary>
       /// <param name="userId"></param>
       /// <returns></returns>
       public static SqlConnection GetConnection(string username)
       {   
           lock(obj)
           {
               if (customConnectionPool.ContainsKey(username)) 
               {
                   if (customConnectionPool[username].State == System.Data.ConnectionState.Open) ///Broken не работает    
                   {                       
                       return customConnectionPool[username];
                   }
                   customConnectionPool.Remove(username);   
               }
               try
               {
                   string cnString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                   var sqlConnection = new SqlConnection(cnString);
                   sqlConnection.Open();
                   customConnectionPool.Add(username, sqlConnection);
                   return sqlConnection;
               }
               catch (Exception ex)
               {
                   ServerSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "(SqlConnection)DataSourceLayer.Gateway.GetConnection(string username)");
                   throw;
               }
           }
       }

       private static object obj = new object();
    }
}
