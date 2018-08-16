namespace Employees.App.Core.Command
{
    using System;
    using System.Globalization;

    using Employees.App.Core.Contracts;
    using Employees.App.Core.Controller;

    public class SetBirthdayCommand : ICommand
    {
        private readonly IEmployeeController employeeController;

        public SetBirthdayCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);
            DateTime dateTime = DateTime.ParseExact(args[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            ;

            employeeController.SetBirthday(employeeId, dateTime);

            return $"Birthday was added successufully.";
        }
    }
}
