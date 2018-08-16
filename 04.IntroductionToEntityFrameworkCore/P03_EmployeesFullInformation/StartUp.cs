using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace P03_EmployeesFullInformation
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .OrderBy(e => e.EmployeeId)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.MiddleName,
                        e.JobTitle,
                        e.Salary
                    })
                    .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P03_EmployeesFullInformation\Result.txt"))
                {
                    foreach (var employee in employees)
                    {
                        sw.WriteLine($"{ employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {Math.Round(employee.Salary, 2)}");
                    }
                }
            }               
        }
    }
}
