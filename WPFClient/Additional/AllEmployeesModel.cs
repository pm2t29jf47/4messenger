using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Additional;

namespace WPFClient.Additional
{
    public class AllEmployeesModel
    {
        List<Employee> employees = new List<Employee>();

        public List<Employee> Employees 
        {
            get
            {
                return employees;
            }
        }

        public void RefreshEmployees()
        {
            List<Entity> entities = new List<Entity>();
            entities.AddRange(employees);
            byte[] recentVersion = AdditionalTools.GetMaxTimestamp(entities);
            EmployeePack pack = App.proxy.GetAllEmployees(recentVersion);
            if (!UpdateEmployeesByPack(pack))
            {
                List<string> usernamesCollection = App.proxy.GetAllEmployeesIds();
                TrimEmployees(usernamesCollection);
            } 
        }

        bool UpdateEmployeesByPack(EmployeePack pack)
        {
            if (pack.Employees.Count > 0)
            {
                Employee employee;
                foreach (Employee item in pack.Employees)
                {
                    employee = Employees.FirstOrDefault(row => string.Compare(row.Username, item.Username) == 0);
                    if (employee == null)
                    {
                        employees.Add(item);
                    }
                    else
                    {
                        employees.Remove(employee);
                        employees.Add(item);
                    }
                }
                /// Восстанавливаем нормальный порядок
                employees = employees.OrderBy(row => row.SecondName).ToList();
            }
            return (employees.Count == pack.CountInDB);
        }

        void TrimEmployees(List<string> usernames)
        {
            List<Employee> removed = new List<Employee>();
            foreach (Employee item in employees)
            {
                if (!usernames.Contains(item.Username))
                {
                    removed.Add(item);
                }
            }
            foreach (Employee item in removed)
            {
                employees.Remove(item);
            }
        }
    }
}
