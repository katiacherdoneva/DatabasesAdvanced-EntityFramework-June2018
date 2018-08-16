namespace Employees.App.Core.Contracts
{
    using System;
    using System.Collections.Generic;

    using Employees.App.Core.Dtos;

    public interface IEmployeeController
    {
        void AddEmployee(EmployeeDto employeeDto);

        void SetBirthday(int employeeId, DateTime dateTime);

        void SetAddress(int employeeId, string address);

        EmployeeDto EmployeeInfo(int employeedId);

        EmployeePersonalInfo EmployeePersonalInfo(int employeedId);

        List<EmployeeOlderThanAge> EmployeesOlderThanAge(int age);
    }
}
