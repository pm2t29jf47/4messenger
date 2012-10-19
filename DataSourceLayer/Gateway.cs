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
           ///Разобраться с многопоточностью!
           ///если подключение закрыли во время использования?
           ///кто закрыает старые подключения?
          // lock(obj)
          // {
               if (customConnectionPool.ContainsKey(username)) 
               {
                   if (customConnectionPool[username].State == System.Data.ConnectionState.Open) ///Broken не работает    
                   {
                       //customConnectionPool[username].
                       return customConnectionPool[username];
                   }
                   customConnectionPool.Remove(username);   
               }
               var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
               sqlConnection.Open();
               customConnectionPool.Add(username, sqlConnection);
               return sqlConnection;  
           //}
       }

       private static object obj = new object();
    }
}
