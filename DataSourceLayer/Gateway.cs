using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DataSourceLayer
{
   public class Gateway
    {
       /// <summary>
       /// Пул подключений
       /// </summary>
       private static Dictionary<string, SqlConnection> CustomConnectionPool
       {
           get
           {
               if (CustomConnectionPool == null)
                   CustomConnectionPool = new Dictionary<string, SqlConnection>();
               return CustomConnectionPool;
           }
           set { }
       }

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
           lock(obj)
           {
               if (CustomConnectionPool.ContainsKey(username)) 
               {
                   if (CustomConnectionPool[username].State == System.Data.ConnectionState.Open) ///Broken не работает          
                       return CustomConnectionPool[username];
                   CustomConnectionPool.Remove(username);   
               }
               var sqlConnection = new SqlConnection();
               sqlConnection.Open();
               CustomConnectionPool.Add(username, sqlConnection);
               return sqlConnection;  
           }
       }

       private static object obj = new object();
    }
}
