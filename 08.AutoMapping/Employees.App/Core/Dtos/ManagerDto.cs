namespace Employees.App.Core.Dtos
{
    using System.Collections.Generic;

    using Employees.Models;

    public class ManagerDto
    {
        public ManagerDto()
        {
            this.Employees = new HashSet<Employee>();
        }

        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
