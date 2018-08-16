using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.IO;
using System.Linq;

namespace P06_AddingaNewAddressandUpdatingEmployee
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var address = new Address()
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };

                var employee = context.Employees
                    .Where(e => e.LastName == "Nakov")
                    .FirstOrDefault();
                employee.Address = address;
                context.SaveChanges();

                var employees = context.Employees
                    .OrderByDescending(e => e.AddressId)
                    .Take(10)
                    .Select(e => new
                    {
                        AddressText = e.Address.AddressText
                    })
                    .ToArray();

                using (StreamWriter sw = new StreamWriter(@"C:\Users\User\source\repos\03.IntroductionToEntityFrameworkCore\P06_AddingaNewAddressandUpdatingEmployee\Result.txt"))
                {
                    foreach (var e in employees)
                    {
                        sw.WriteLine($"{e.AddressText}");
                    }
                }
            }
        }
    }
}
