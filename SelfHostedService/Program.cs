using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DBService;

namespace SelfHostedService
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a URI to serve as the base address
            
            //Create ServiceHost
            ServiceHost host = new ServiceHost(typeof(Service1));
            //Add a service endpoint
            //host.AddServiceEndpoint(typeof(MyCalculatorService.ISimpleCalculator)
           // , new WSHttpBinding(), "");
            //Enable metadata exchange
           // ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
          //  smb.HttpGetEnabled = true;
          //  host.Description.Behaviors.Add(smb);
            //Start the Service
            host.Open();

            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press <Enter> key to stop");
            Console.ReadLine();
        }
    }
}
