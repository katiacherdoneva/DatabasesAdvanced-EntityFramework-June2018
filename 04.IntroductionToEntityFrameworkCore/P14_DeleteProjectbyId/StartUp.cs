using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace P14_DeleteProjectbyId
{
   public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var project = context.Projects.Find(2);

                var projectInEmployeesProjects = context.EmployeesProjects
                    .Where(ep => ep.ProjectId == 2)
                    .ToArray();
                context.EmployeesProjects.RemoveRange(projectInEmployeesProjects);

                context.Projects.Remove(project);
                context.SaveChanges();

                var projects = context.Projects
                    .Select(p => new
                    {
                        p.Name
                    })
                    .Take(10)
                    .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P14_DeleteProjectbyId\Result.txt"))
                {
                    foreach (var p in projects)
                    {
                        sw.WriteLine($"{p.Name}");
                    }
                }
            }
        }
    }
}
