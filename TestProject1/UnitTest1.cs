using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSourceLayer;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = MessageGateway.SelectMessage(1, "ivan");
            var b = MessageGateway.SelectMessage(2, "ivan");
        }
    }
}
