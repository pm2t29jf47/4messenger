using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSourceLayer;
using DBService;
using WPFClient.Additional;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] a = new string[5] { "aaa", "sss", "ddd", "fff", "aaa" };
            List<string> al = new List<string>();
            string msg = string.Empty;
            foreach (var item in a)
            {
                if(al.Contains(item, new CustomStringComparer()))
                {
                    msg += item + ")";
                }
                else
                {
                    al.Add(item);
                }
            }
            int v = 2;
            
 
    
 
            
        }
    }
}
