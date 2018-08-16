using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace P04_EmployeeswithSalaryOver50000
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => e.Salary > 50000)
                    .OrderBy(e => e.FirstName)
                    .Select(e => new
                    {
                        e.FirstName
                    })
                    .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P04_EmployeeswithSalaryOver50000\Result.txt"))
                {
                    foreach (var employee in employees)
                    {
                        sw.WriteLine($"{employee.FirstName}");
                    }
                }
            }
        }
    }
}
