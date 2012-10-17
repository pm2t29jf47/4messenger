using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DBService;

namespace TestConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory1 = new ChannelFactory<IService1>("*");
            factory1.Credentials.UserName.UserName = "bob";
            factory1.Credentials.UserName.Password = "boba";

            var proxy = factory1.CreateChannel();
           // var a = proxy.GetTrue();
            Entities.Employee a = proxy.GetNewEmployee();
        }
    }
}
