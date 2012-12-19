using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using EFDataSourceLayer;
using Entities;

namespace DBService
{  
    class PasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string username, string password)
        {
            Employee securityEmloyee = EmployeeGateway.SelectSecurity(username, "PasswordValidatorAccount"); ///PasswordValidatorAccount нужен для Geteway.customConnectionPool
            if (securityEmloyee != null)
            {
                if (string.Compare(securityEmloyee.Password, password) == 0)
                    return;
            }
            throw new SecurityTokenValidationException();           
        }
    }
}
