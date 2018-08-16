namespace Employees.App
{
    using System;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;

    using Employees.Data;
    using Employees.Services.Contracts;
    using Employees.Services;
    using Employees.App.Core.Contracts;
    using Employees.App.Core;
    using Employees.App.Core.Controller;
    using AutoMapper;

    public class StartUp
    {
        public static void Main()
        {
            var service = ConfigreService();
            IEngine engine = new Engine(service);
            engine.Run();
        }

        private static IServiceProvider ConfigreService()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeesContext>(opts => opts.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddTransient<IDbInitializerService, DbInitializerService>();

            serviceCollection.AddTransient<ICommandIntrepreter, CommandIntrepreter>();

            serviceCollection.AddTransient<IEmployeeController, EmployeeController>();

            serviceCollection.AddTransient<IManagerController, ManagerController>();

            serviceCollection.AddAutoMapper(conf=> conf.AddProfile<EmployeesProfile>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
