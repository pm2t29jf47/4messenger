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
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CustomDbContext>());
            string cnString = ConfigurationManager.ConnectionStrings["EFDB"].ConnectionString;
            CustomDbContext dc = new CustomDbContext(cnString);
            dc.Database.Initialize(false);
            var a = dc.Messages.FirstOrDefault();
           Message m = new Message()
                                    {
                                        Content="CCCC",
                                        Date = DateTime.Today,
                                        Deleted = false,
                                        SenderUsername = "ivan",
                                        Title = "TTTT"   ,
                                        LastUpdate = DateTime.Now,
                                        Recipients = new List<Recipient>()
                                        {
                                            new Recipient("ivan1", null)
                                            {
                                                Deleted = false,
                                                Viewed = false,
                                                Message = new Message()
                                                {
                                                    Content="DDDD",
                                                    Date = DateTime.Today,
                                                    Deleted = false,
                                                    SenderUsername = "ivan",
                                                    Title = "FFFF"   ,
                                                    LastUpdate = DateTime.Now,
                                                }
                                            }
                                        }
                                    };
           dc.Messages.Add(m);
           
            dc.SaveChanges();
            var i = dc.Messages.Find(m);
           
        }
    }
}
