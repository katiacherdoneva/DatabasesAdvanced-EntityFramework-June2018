using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace P12_IncreaseSalaries
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => e.Department.Name == "Engineering"
                    || e.Department.Name == "Tool Design"
                    || e.Department.Name == "Marketing"
                    || e.Department.Name == "Information Services")
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P12_IncreaseSalaries\Result.txt"))
                {
                    foreach (var e in employees)
                    {
                        e.Salary *= (decimal)1.12;

                        sw.WriteLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
