using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace P13_FindEmployeesbyFirstNameStartingWithSa
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                     .Where(e => EF.Functions.Like(e.FirstName, "Sa%"))
                    //.Where(e => e.FirstName.Substring(0, 2) == "Sa")
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        e.Salary
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P13_FindEmployeesbyFirstNameStartingWithSa\Result.txt"))
                {
                    foreach (var e in employees)
                    {
                        sw.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
                    }
                }
            }
        }
    }
}
