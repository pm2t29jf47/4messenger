using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Threading;
using System.Collections;

namespace WPFClient.Additional
{
    public class ServiceWatcher
    {
        public ServiceWatcher() 
        {
            DataUpdated += new eventHandler(OnServiceWatcherDataUpdated);
        }

        public void StartWathing()
        {///im watching u!
            AllEmployees = App.Proxy.GetAllEmployees();
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += new EventHandler(OntimerTick);
            timer.Start();
        }

        delegate void eventHandler();

        event eventHandler DataUpdated;

        void OnServiceWatcherDataUpdated()
        {
            int a = 10;
        }

       
        List<Employee> allEmployees;    

        public List<Employee> AllEmployees
        {
            get
            {
              //  lock (((ICollection)allEmployees).SyncRoot)
               // {
                    return allEmployees;
                //}
            }
            set
            {               
                    if(allEmployees == value)
                        return;

                    allEmployees = value;
                
            }
        
        }

        void OntimerTick(object sender, EventArgs e)
        {
            if (DataUpdated != null)
                DataUpdated();
        }
    }
}
