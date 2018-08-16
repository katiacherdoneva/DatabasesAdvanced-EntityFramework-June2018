namespace Employees.App.Core.Command
{
    using System;
    using System.Globalization;

    using Employees.App.Core.Contracts;
    using Employees.App.Core.Controller;

    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly IEmployeeController employeeController;

        public EmployeePersonalInfoCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var employeePersonalInfoDto = this.employeeController.EmployeePersonalInfo(employeeId);

            string employeeBirthday = employeePersonalInfoDto.Birthday?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return $"ID: {employeePersonalInfoDto.EmployeeId} - {employeePersonalInfoDto.FirstName} {employeePersonalInfoDto.LastName} - ${employeePersonalInfoDto.Salary:f2}"
                + Environment.NewLine + $"Birthday: {employeeBirthday}"
                + Environment.NewLine + $"Address: {employeePersonalInfoDto.Address}";
        }
    }
}
