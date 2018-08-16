using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace P15_RemoveTowns
{
    public class StartUp
    {
        static void Main()
        {
            string townName = Console.ReadLine();

            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => e.Address.Town.Name == townName)
                    .ToArray();
                foreach (var e in employees)
                {
                    e.AddressId = null;
                }

                var addresses = context.Addresses
                  .Where(a => a.Town.Name == townName)
                  .ToArray();
                context.Addresses.RemoveRange(addresses);

                var town = context.Towns
                  .Where(t => t.Name == townName).FirstOrDefault();
                context.Towns.Remove(town);

                Console.WriteLine($"{addresses.Count()} address in {townName} was deleted");
                context.SaveChanges();
            }
        }
    }
}
