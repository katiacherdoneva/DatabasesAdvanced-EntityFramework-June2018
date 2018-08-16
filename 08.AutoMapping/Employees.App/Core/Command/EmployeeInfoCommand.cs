namespace Employees.App.Core.Command
{
    using Employees.App.Core.Contracts;
    using Employees.App.Core.Controller;

    public class EmployeeInfoCommand : ICommand
    {
        private readonly IEmployeeController employeeController;

        public EmployeeInfoCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var employeeDto = this.employeeController.EmployeeInfo(employeeId);

            return $"ID: {employeeDto.EmployeeId} - {employeeDto.FirstName} {employeeDto.LastName} -  ${employeeDto.Salary:F2}";
        }
    }
}
