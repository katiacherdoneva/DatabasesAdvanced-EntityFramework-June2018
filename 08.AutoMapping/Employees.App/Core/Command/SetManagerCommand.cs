namespace Employees.App.Core.Command
{
    using Employees.App.Core.Contracts;

    public class SetManagerCommand : ICommand
    {
        private readonly IManagerController controller;

        public SetManagerCommand(IManagerController controller)
        {
            this.controller = controller;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);
            int managerId = int.Parse(args[1]);

            controller.SetManager(employeeId, managerId);

            return $"Manager was added successfully on employee with ID: {employeeId}";
        }
    }
}
