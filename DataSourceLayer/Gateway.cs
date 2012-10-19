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
       private static Dictionary<int, SqlConnection> CustomConnectionPool
       {
           get
           {
               if (CustomConnectionPool == null)
                   CustomConnectionPool = new Dictionary<int, SqlConnection>();
               return CustomConnectionPool;
           }
           set { }
       }

       /// <summary>
       /// Возвращает подключение из пула
       /// </summary>
       /// <param name="userId"></param>
       /// <returns></returns>
       public static SqlConnection GetConnection(int userId)
       {
           ///Разобраться с многопоточностью!
           ///если подключение закрыли во время использования?
           ///кто закрыает старые подключения?
           lock(obj)
           {
               if(CustomConnectionPool.ContainsKey(userId)) 
               {
                   if(CustomConnectionPool[userId].State == System.Data.ConnectionState.Open) ///Broken не работает          
                       return CustomConnectionPool[userId];
                   CustomConnectionPool.Remove(userId);   
               }
               var sqlConnection = new SqlConnection();
               sqlConnection.Open();
               CustomConnectionPool.Add(userId, sqlConnection);
               return sqlConnection;  
           }
       }

       private static object obj = new object();
    }
}
