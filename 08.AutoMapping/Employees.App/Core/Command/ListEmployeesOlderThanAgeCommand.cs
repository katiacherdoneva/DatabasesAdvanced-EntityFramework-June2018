namespace Employees.App.Core.Command
{
    using System.Collections.Generic;
    using System.Text;

    using Employees.App.Core.Contracts;
    using Employees.App.Core.Dtos;

    public class ListEmployeesOlderThanAgeCommand : ICommand
    {
        private readonly IEmployeeController employeeController;

        public ListEmployeesOlderThanAgeCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int age = int.Parse(args[0]);

            List<EmployeeOlderThanAge> employeeOlderThanAge = this.employeeController.EmployeesOlderThanAge(age);

            StringBuilder sb = new StringBuilder();
            foreach (var e in employeeOlderThanAge)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - ${e.Salary:f2} - Manager: {e.ManagerId}");
            }

            return sb.ToString().Trim();
        }
    }
}
