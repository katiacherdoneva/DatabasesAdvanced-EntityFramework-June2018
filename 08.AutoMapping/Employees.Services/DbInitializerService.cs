namespace Employees.Services
{
    using Employees.Data;
    using Microsoft.EntityFrameworkCore;
    using Services.Contracts;

    public class DbInitializerService : IDbInitializerService
    {
        private readonly EmployeesContext context;

        public DbInitializerService(EmployeesContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.Migrate();
        }
    }
}
