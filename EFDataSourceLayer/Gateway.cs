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
              
       /// <summary>
       /// Возвращает контекст из пула
       /// </summary>
       /// <param name="username"></param>
       /// <returns></returns>
       public static CustomDbContext GetContext(string username)
       {
           lock (obj)
           {
               if (customDbContextPool.ContainsKey(username))
               {
                   var a = customDbContextPool[username].Database.Connection;
                   if (customDbContextPool[username].Database.Connection.State == System.Data.ConnectionState.Open) ///Broken не работает    
                   {
                       return customDbContextPool[username];
                   }
                   customDbContextPool.Remove(username);
               }
               try
               {                   
                   string cnString = ConfigurationManager.ConnectionStrings["EFDB"].ConnectionString;
                   CustomDbContext dc = new CustomDbContext(cnString);
                   //dc.Database.Initialize(false);
                   customDbContextPool.Add(username, dc);
                   return dc;
               }
               catch (Exception ex)
               {
                   ServerSideExceptionHandler.ExceptionHandler.HandleExcepion(ex, "(CustomDbContext)EFDataSourceLayer.Gateway.GetContext(string username)");
                   throw;
               }
           }          
       }

       private static object obj = new object();
    }
}
