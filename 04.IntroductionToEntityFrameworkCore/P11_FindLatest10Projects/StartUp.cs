using P02_DatabaseFirst.Data;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace P11_FindLatest10Projects
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var projects = context.Projects
                    .OrderByDescending(p => p.StartDate)
                    .Take(10)
                    .Select(p => new
                    {
                        p.Name,
                        p.Description,
                        p.StartDate
                    })
                    .OrderBy(p => p.Name)
                    .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P11_FindLatest10Projects\Result.txt"))
                {
                    foreach (var p in projects)
                    {
                        sw.WriteLine(p.Name);
                        sw.WriteLine(p.Description);
                        sw.WriteLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
                    }
                }
            }
        }
    }
}
