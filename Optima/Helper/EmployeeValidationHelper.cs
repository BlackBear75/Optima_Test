using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Optima.Helper
{
    public static class EmployeeValidationHelper
    {
        public static bool ValidateName(string name, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                errorMessage = "Name cannot be empty.";
                return false;
            }

            if (name.Length > 50)
            {
                errorMessage = "Name cannot be longer than 50 characters.";
                return false;
            }

            if (Regex.IsMatch(name, @"\d"))
            {
                errorMessage = "Name cannot contain numbers.";
                return false;
            }

            errorMessage = null;
            return true;
        }

        public static bool ValidateSalary(string salaryText, out string errorMessage)
        {
            if (!double.TryParse(salaryText, out var salary) || salary <= 0)
            {
                errorMessage = "Salary must be a positive number.";
                return false;
            }

            errorMessage = null;
            return true;
        }

      
    }

}
