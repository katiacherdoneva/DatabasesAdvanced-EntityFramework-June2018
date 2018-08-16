using System.Collections.Generic;

namespace Employees.App.Core.Contracts
{
    public interface ICommand
    {
        string Execute(string[] args);
    }
}
