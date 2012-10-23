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
            factory1.Credentials.UserName.UserName = "Admin";
            factory1.Credentials.UserName.Password = "22h2";
            var proxy = factory1.CreateChannel();
            //var r = new List<Entities.Recipient>();
            //r.Add(new Entities.Recipient("Ivan",null,false));
            //r.Add(new Entities.Recipient("Ivan1",null,false));
            //proxy.SendMessage(
            //    new Entities.Message(
            //        null,
            //        "заголовок",
            //        DateTime.Now,
            //        r,
            //        "admin",
            //        "письмо письмо письмо письмо"));
            //r.RemoveAt(1);
            //proxy.SendMessage(
            //    new Entities.Message(
            //        null,
            //        "заголовок2",
            //        DateTime.Now,
            //        r,
            //        "admin",
            //        "письмо письмо письмо только ивану"));
            var a = proxy.InboxMessages();
        
            
        }
    }
}
