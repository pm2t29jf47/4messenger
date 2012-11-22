using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;
using WPFClient.Properties;

namespace WPFClient.OverEntities
{
    public class EmployeeModel
    {
        Employee employee = new Employee();

        public Employee Employee
        {
            get
            {
                return employee;
            }
            set
            {
                if (employee == null)
                    return;

                employee = value;
            }
        }

        public string LongString
        {
            get
            {
                string result = employee.FirstName
                    + SpecialSymbols.SpecialSymbols.space
                    + employee.SecondName
                    + SpecialSymbols.SpecialSymbols.space
                    + SpecialSymbols.SpecialSymbols.leftUsernameStopper
                    + employee.Username
                    + SpecialSymbols.SpecialSymbols.rightUsernameStopper;

                if (string.Compare(App.Username, employee.Username) == 0)
                {
                    return Resources.Me
                        + SpecialSymbols.SpecialSymbols.space
                        + SpecialSymbols.SpecialSymbols.leftTitleStopper
                        + result
                        + SpecialSymbols.SpecialSymbols.rightTitleStopper;
                }
                return result;
            }
        }
    }
}
