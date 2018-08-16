namespace Employees.App.Core
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using Employees.App.Core.Contracts;
    using Employees.Services.Contracts;
    using System.Linq;

    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            var initializeDb = this.serviceProvider.GetService<IDbInitializerService>();
            initializeDb.InitializeDatabase();

            var commandIntrepreter = this.serviceProvider.GetService<ICommandIntrepreter>();

            while (true)
            {
                string[] input = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                try
                {
                    string result = commandIntrepreter.Read(input);
                    Console.WriteLine(result);
                }
                catch(ArgumentException argEx)
                {
                    Console.WriteLine(argEx.Message);
                }
            }
        }
    }
}
