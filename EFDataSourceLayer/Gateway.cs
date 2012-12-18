using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity;
using EFDataSourceLayer;

namespace EFDataSourceLayer
{
   public class Gateway
    {
       /// <summary>
       /// Пул контекстов
       /// </summary>
       private static Dictionary<string, CustomDbContext> customDbContextPool = new Dictionary<string, CustomDbContext>();       

       ///// <summary>
       ///// Возвращает подключение из пула
       ///// </summary>
       ///// <param name="userId"></param>
       ///// <returns></returns>
       //public static SqlConnection GetConnection(string username)
       //{   
       //    lock(obj)
       //    {
       //        if (customConnectionPool.ContainsKey(username)) 
       //        {
       //            if (customConnectionPool[username].State == System.Data.ConnectionState.Open) ///Broken не работает    
       //            {                       
       //                return customConnectionPool[username];
       //            }
       //            customConnectionPool.Remove(username);   
       //        }
       //        try
       //        {
       //            string cnString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
       //            var sqlConnection = new SqlConnection(cnString);
       //            sqlConnection.Open();
       //            customConnectionPool.Add(username, sqlConnection);
       //            return sqlConnection;
       //        }
       //        catch (Exception ex)
       //        {
       //            ServerSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "(SqlConnection)DataSourceLayer.Gateway.GetConnection(string username)");
       //            throw;
       //        }
       //    }
       //}

       public static CustomDbContext GetContext(string username)
       {
           lock (obj)
           {
               if (customDbContextPool.ContainsKey(username))
               {
                   if (customDbContextPool[username].Database.Connection.State == System.Data.ConnectionState.Open) ///Broken не работает    
                   {
                       return customDbContextPool[username];
                   }
                   customDbContextPool.Remove(username);
               }
               try
               {
                   //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CustomDbContext>());
                   string cnString = ConfigurationManager.ConnectionStrings["EFDB"].ConnectionString;
                   CustomDbContext dc = new CustomDbContext(cnString);
                   dc.Database.Initialize(false);
                   customDbContextPool.Add(username, dc);
                   return dc;
               }
               catch (Exception ex)
               {
                   ServerSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "(SqlConnection)EFDataSourceLayer.Gateway.CustomDbContext GetContext(string username)");
                   throw;
               }
           }
           return null;
       }

       private static object obj = new object();
    }
}
