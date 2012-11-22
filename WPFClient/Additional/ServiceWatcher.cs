using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Threading;

namespace WPFClient.Additional
{
    public class ServiceWatcher
    {
        public ServiceWatcher() { }

        public void StartWathing()
        {///im watching u!
            allEmployees = App.Proxy.GetAllEmployees();
            Mutex d = new Mutex()

        }
  
        List<Employee> allEmployees;
    

        public List<Employee> AllEmployees
        {
            
        
            get;
            set;
        
        }
    }
}
