using Employees.App.Core.Contracts;
using Employees.App.Core.Dtos;
using System.Text;

namespace Employees.App.Core.Command
{
    public class ManagerInfoCommand : ICommand
    {
        private readonly IManagerController controller;

        public ManagerInfoCommand(IManagerController controller)
        {
            this.controller = controller;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            ManagerDto managerDto = controller.ManagerInfo(employeeId);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.Employees.Count}");

            foreach (var e in managerDto.Employees)
            {
                stringBuilder.AppendLine($"   - {e.FirstName} {e.LastName} - ${e.Salary:f2}");
            }

            return stringBuilder.ToString().Trim();
        }
    }
}
