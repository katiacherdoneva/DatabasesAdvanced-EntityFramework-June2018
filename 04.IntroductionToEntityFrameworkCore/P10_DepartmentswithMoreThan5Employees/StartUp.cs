using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace P10_DepartmentswithMoreThan5Employees
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var departments = context.Departments
                    .Where(d => d.Employees.Count() > 5)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d => d.Name)
                    .Select(d => new
                    {
                        d.Name,
                        ManagerFirstName = d.Manager.FirstName,
                        ManagerLastName = d.Manager.LastName,
                        Employees = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName, 
                            e.JobTitle
                        })
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .ToArray()
                    })
                    .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P10_DepartmentswithMoreThan5Employees\Result.txt"))
                {
                    foreach (var d in departments)
                    {
                        sw.WriteLine($"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}");

                        foreach (var e in d.Employees)
                        {
                            sw.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                        }
                        sw.WriteLine("----------");
                    }
                }
            }
        }
    }
}
