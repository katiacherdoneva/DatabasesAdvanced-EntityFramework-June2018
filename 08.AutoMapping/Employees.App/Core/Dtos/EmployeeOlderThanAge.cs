using System;
using System.Collections.Generic;
using System.Text;

namespace Employees.App.Core.Dtos
{
    public class EmployeeOlderThanAge
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime Birthday { get; set; }

        public int ManagerId { get; set; }
    }
}
