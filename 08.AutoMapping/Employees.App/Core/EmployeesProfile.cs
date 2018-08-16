namespace Employees.App
{
    using AutoMapper;

    using Employees.App.Core.Dtos;
    using Employees.Models;

    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, EmployeePersonalInfo>().ReverseMap();
            CreateMap<Employee, ManagerDto>().ReverseMap();
            CreateMap<Employee, EmployeeOlderThanAge>().ReverseMap();
        }
    }
}
