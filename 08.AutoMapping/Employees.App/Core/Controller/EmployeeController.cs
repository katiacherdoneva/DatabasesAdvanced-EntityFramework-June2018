namespace Employees.App.Core.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Employees.App.Core.Contracts;
    using Employees.App.Core.Dtos;
    using Employees.Data;
    using Employees.Models;

    public class EmployeeController : IEmployeeController
    {
        private readonly EmployeesContext context;
        private readonly IMapper mapper;

        public EmployeeController(EmployeesContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddEmployee(EmployeeDto employeeDto)
        {
            var employee = mapper.Map<Employee>(employeeDto);

            context.Employees.Add(employee);

            context.SaveChanges();
        }

        public void SetBirthday(int employeeId, DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentException("Invalid dateTime");
            }

            var employee = context.Employees.Find(employeeId);
            if (employee == null)
            {
                throw new ArgumentException("Invalid Id.");
            }

            employee.Birthday = dateTime;

            context.SaveChanges();
        }

        public void SetAddress(int employeeId, string address)
        {
            if (address == null)
            {
                throw new ArgumentException("Invalid address.");
            }

            var employee = context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid Id.");
            }

            employee.Address = address;

            context.SaveChanges();
        }

        public EmployeeDto EmployeeInfo(int employeeId)
        {
            var employee = context.Employees
                .Find(employeeId);

            var employeeDto = mapper.Map<EmployeeDto>(employee);

            if (employeeDto == null)
            {
                throw new ArgumentException("Invalid Id.");
            }
            return employeeDto;
        }

        public EmployeePersonalInfo EmployeePersonalInfo(int employeeId)
        {
            var employee = context.Employees
               .Find(employeeId);

            var employeePersonalInfo = mapper.Map<EmployeePersonalInfo>(employee);

            if (employeePersonalInfo == null)
            {
                throw new ArgumentException("Invalid Id.");
            }

            return employeePersonalInfo;
        }

        public List<EmployeeOlderThanAge> EmployeesOlderThanAge(int age)
        {
            var employees = context.Employees
                .Where(x => (DateTime.Now.Year - x.Birthday.Value.Year) > age)
                .ToArray();

            if (0 == employees.Length)
            {
                throw new ArgumentException("No employees older than age.");
            }

            List<EmployeeOlderThanAge> employeesOlderThanAge = new List<EmployeeOlderThanAge>();
            foreach (var e in employees)
            {
                employeesOlderThanAge.Add(mapper.Map<EmployeeOlderThanAge>(e));
            }

            return employeesOlderThanAge;
        }
    }
}
