namespace Employees.App.Core.Command
{
    using Employees.App.Core.Contracts;
    using Employees.App.Core.Controller;

    public class SetAddressCommand : ICommand
    {
        private readonly IEmployeeController employeeController;

        public SetAddressCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);
            string address = args[1];

            employeeController.SetAddress(employeeId, address);

            return $"Address was added successufully.";
        }
    }
}