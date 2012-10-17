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
            factory1.Credentials.UserName.UserName = "administrator";
            factory1.Credentials.UserName.Password = "administrator";

            var proxy = factory1.CreateChannel();
            var a = proxy.GetTrue();
            Entities.Employee an;
            try
            {
                 an = proxy.GetNewEmployee();
            }
            catch (Exception e)
            {
                int ab = 10;
            }
            int bd = 11111;
        }
    }
}
