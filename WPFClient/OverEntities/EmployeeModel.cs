using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;

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

        public string LongEmployeeName
        {
            get
            {
                string result = employee.FirstName
                    + SpecialSymbols.space
                    + employee.SecondName
                    + SpecialSymbols.space
                    + SpecialSymbols.leftUsernameStopper
                    + employee.Username
                    + SpecialSymbols.rightUsernameStopper;

                if (string.Compare(App.Username, employee.Username) == 0)
                {
                    return Properties.Resources.Me
                        + SpecialSymbols.space
                        + SpecialSymbols.leftTitleStopper
                        + result
                        + SpecialSymbols.rightTitleStopper;
                }
                return result;
            }
        }
    }
}
