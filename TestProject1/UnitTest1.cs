using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSourceLayer;
using DBService;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string left = "<", right = ">";
            string a = "Иван Иванов <Ivan>";
            int start = a.IndexOf(left[0]);
            int end = a.IndexOf(right[0]);
 
    
 
            
        }
    }
}
