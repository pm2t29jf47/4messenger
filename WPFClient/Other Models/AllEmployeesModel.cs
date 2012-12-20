using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Entities.Additional;
using WPFClient.Additional;

namespace WPFClient.OtherModels
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
            List<Employee> employeesBuf = new List<Employee>();
            employeesBuf.AddRange(employees);          
            if (!UpdateEmployeesBufByPack(pack, employeesBuf))
            {
                TrimEmployees(employeesBuf);
            }
            UploadFromEmployeesBuf(employeesBuf);
        }

        void UploadFromEmployeesBuf(List<Employee> employeesBuf)
        {
            employees.Clear();
            employees.AddRange(employeesBuf);
        }

        bool UpdateEmployeesBufByPack(EmployeePack pack, List<Employee> employeesBuf)
        {
            if (pack.Employees.Count > 0)
            {                
                foreach (Employee item in pack.Employees)
                {
                    Employee employee = employeesBuf.FirstOrDefault(row => string.Compare(row.Username, item.Username) == 0);
                    if (employee == null)
                    {
                        employeesBuf.Add(item);
                    }
                    else
                    {
                        employeesBuf.Remove(employee);
                        employeesBuf.Add(item);
                    }
                }
                /// Восстанавливаем нормальный порядок
                employeesBuf = employeesBuf.OrderBy(row => row.SecondName).ToList();
            }
            return (employeesBuf.Count == pack.CountInDB);
        }

        void TrimEmployees(List<Employee> employeesBuf)
        {
            List<string> usernamesCollection = App.proxy.GetAllEmployeesIds();
            List<Employee> removed = new List<Employee>();
            foreach (Employee item in employeesBuf)
            {
                if (!usernamesCollection.Contains(item.Username))
                {
                    removed.Add(item);
                }
            }
            foreach (Employee item in removed)
            {
                employeesBuf.Remove(item);
            }
        }
    }
}
