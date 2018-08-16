namespace Employees.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Employees.App.Core.Contracts;
    using Microsoft.Extensions.DependencyInjection;

    public class CommandIntrepreter : ICommandIntrepreter
    {
        private readonly IServiceProvider serviceProvider;

        public CommandIntrepreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string Read(string[] args)
        {
            string commandName = args[0] + "Command";

            var type = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(x => x.Name == commandName);

            if(type == null)
            {
                throw new ArgumentException("Invalid input.");
            }

            var constructor = type.GetConstructors()
                .First();

            var constructorParameters = constructor.GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();

            var service = constructorParameters.Select(serviceProvider.GetService)
                .ToArray();

            var command = (ICommand)constructor.Invoke(service);

            string[] onlyArgs = args.Skip(1).ToArray();
            string result = command.Execute(onlyArgs);

            return result;
        }
    }
}
