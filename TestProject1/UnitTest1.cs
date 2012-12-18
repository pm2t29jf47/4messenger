using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSourceLayer;
using DBService;
using WPFClient.Additional;
using Entities;
using EFDataSourceLayer;
using System.Data.Entity;
using System.Configuration;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CustomDbContext>());
            string cnString = ConfigurationManager.ConnectionStrings["EFDB"].ConnectionString;
            CustomDbContext dc = new CustomDbContext(cnString);
            dc.Database.Initialize(false);
            Employee a = dc.Employees.FirstOrDefault();            

          //  var a = EFDataSourceLayer.EmployeeGateway.SelectAll("a");

            
          
           

                
        }



   
    }
}
