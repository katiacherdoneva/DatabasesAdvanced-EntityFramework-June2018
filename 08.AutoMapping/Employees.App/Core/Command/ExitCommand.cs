namespace Employees.App.Core.Command
{
    using System;
    using System.Threading;
    using Employees.App.Core.Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] args)
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Program will close after {i} seconds.");
                Thread.Sleep(1000);
            }

            Environment.Exit(0);
            return null;
        }
    }
}
