namespace Employees.App.Core.Controller
{
    using System;

    using AutoMapper;

    using Employees.App.Core.Contracts;
    using Employees.App.Core.Dtos;
    using Employees.Data;

    public class ManagerController : IManagerController
    {
        private readonly EmployeesContext context;
        private readonly IMapper mapper;

        public ManagerController(EmployeesContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public ManagerDto ManagerInfo(int employeeId)
        {
            var employee = context.Employees.Find(employeeId);
            if (employee == null)
            {
                throw new ArgumentException("Invalid employee id.");
            }

            var managerDto = mapper.Map<ManagerDto>(employee);

            return managerDto;
        }

        public void SetManager(int employeeId, int managerId)
        {
            var employee = context.Employees.Find(employeeId);
            if(employee == null)
            {
                throw new ArgumentException("Invalid employee id.");
            }

            employee.ManagerId = managerId;
            context.SaveChanges();
        }    
    }
}
