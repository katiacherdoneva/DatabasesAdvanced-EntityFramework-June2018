namespace Employees.App.Core.Contracts
{
    using Employees.App.Core.Dtos;

    public interface IManagerController
    {
        void SetManager(int employeeId, int managerId);

        ManagerDto ManagerInfo(int employeeId);
    }
}
