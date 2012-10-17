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
       /// Пу подключений
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
       /// <param name="UserId"></param>
       /// <returns></returns>
       public static SqlConnection GetConnection(int UserId)
       {
           if(CustomConnectionPool.ContainsKey(UserId)) 
           {
               var result = CustomConnectionPool[UserId];
               if(result.State == System.Data.ConnectionState.Open) ///Broken не работает          
                   return result;
               CustomConnectionPool.Remove(UserId);   
           }
           var sqlConnection = new SqlConnection();
           sqlConnection.Open();
           CustomConnectionPool.Add(UserId, sqlConnection);
           return sqlConnection;           
       }       
    }
}
