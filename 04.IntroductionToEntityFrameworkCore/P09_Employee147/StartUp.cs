using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace P09_Employee147
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employee = context.Employees
                    .Where(e => e.EmployeeId == 147)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        Projects = e.EmployeesProjects.Select(p => new
                        {
                            ProjectName = p.Project.Name
                        })
                        .OrderBy(p => p.ProjectName)
                        .ToArray()
                    }).FirstOrDefault();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P09_Employee147\Result.txt"))
                {
                    sw.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

                    foreach (var p in employee.Projects)
                    {
                        sw.WriteLine(p.ProjectName);
                    }
                }
            }
        }
    }
}
