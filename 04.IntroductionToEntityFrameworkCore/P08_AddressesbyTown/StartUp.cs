using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace P08_AddressesbyTown
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var addresses = context.Addresses
                         .Select(a => new
                         {
                             a.AddressText,
                             TownName = a.Town.Name,
                             EmployeeCount = a.Employees.Count()
                         })
                         .OrderByDescending(e => e.EmployeeCount)
                         .ThenBy(e => e.TownName)
                         .ThenBy(e => e.AddressText)
                         .Take(10)
                         .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P08_AddressesbyTown\Result.txt"))
                {
                    foreach (var a in addresses)
                    {
                        sw.WriteLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
                    }
                }
            }
        }
    }
}
