using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSourceLayer;
using Entities;
using DBWcfService;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = new Service1();
            a.SendMessage(
                new Message(
                    0,
                    "first title",
                    DateTime.Now,
                    new List<Recipient>(
                        new Recipient[] {
                            new Recipient(9, -1, false),
                            new Recipient(10, -1, false),
                            new Recipient(11, -1, true)
                        }),
                        new Employee(13, "Nfvvvv"),
                        "contentcontent"));
 
        }
    }
}
