﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSourceLayer;
using Entities;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = new RecipientGateway();
            a.InsertRecipient(new Recipient(6, 6, false));
            int b = 10;
        }
    }
}
