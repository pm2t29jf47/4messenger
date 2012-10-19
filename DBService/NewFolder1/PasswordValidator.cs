﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace DBService
{
    // validator that checks that username == password
    class PasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string username, string password)
        {
            if (username != password)
            {
                throw new SecurityTokenValidationException();
            }
        }
    }
}
