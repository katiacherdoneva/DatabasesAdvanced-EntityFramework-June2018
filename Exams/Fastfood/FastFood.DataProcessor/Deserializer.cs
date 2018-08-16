using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            StringBuilder sb = new StringBuilder();
            var deserializerEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            List<Employee> employees = new List<Employee>();
            foreach (var employeeDto in deserializerEmployees)
            {
                bool isExistsEmployee = employees.Any(x => x.Name == employeeDto.Name);
                if(!IsValid(employeeDto) || isExistsEmployee)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = GetPosition(employeeDto.Position, context);

                var employee = new Employee()
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = position
                };

                employees.Add(employee);
                sb.AppendLine(String.Format(SuccessMessage, employee.Name));
            }
            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

        private static Position GetPosition(string positionName, FastFoodDbContext context)
        {
            var position = context.Positions
                .Where(x => x.Name == positionName).FirstOrDefault();

            if(position == null)
            {
                var newPosition = new Position()
                {
                    Name = positionName
                };

                context.Positions.Add(newPosition);
                context.SaveChanges();

                return newPosition;
            }

            return position;
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
		{
            StringBuilder sb = new StringBuilder();

            var deserializerItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            List<Item> items = new List<Item>();
            foreach (var itemDto in deserializerItems)
            {
                bool isExistsItem = items.Any(x => x.Name == itemDto.Name);
                if(!IsValid(itemDto) || isExistsItem)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var category = GetCategory(context, itemDto.Category);

                var item = new Item()
                {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Category = category
                };

                items.Add(item);
                sb.AppendLine(String.Format(SuccessMessage, item.Name));
            }
            context.Items.AddRange(items);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

        private static Category GetCategory(FastFoodDbContext context, string categoryName)
        {
            var category = context.Categories
                .Where(x => x.Name == categoryName).FirstOrDefault();

            if (category == null)
            {
                var newCategory = new Category()
                {
                    Name = categoryName
                };

                context.Categories.Add(newCategory);
                context.SaveChanges();

                return newCategory;
            }

            return category;
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var deserializerOrders = (OrderDto[])serializer.Deserialize(new StringReader(xmlString));

            foreach (var orderDto in deserializerOrders)
            {
                var isValidDateTime = DateTime.TryParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime dateTime);
                if (!IsValid(orderDto) || !isValidDateTime)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var employee = context.Employees.Where(x => x.Name == orderDto.EmployeeName)
                    .FirstOrDefault();
                if(employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var date = DateTime
                   .ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                var order = new Order()
                {
                    Customer = orderDto.Customer,
                    Employee = employee,
                    DateTime = date,
                    Type = (OrderType)Enum.Parse(typeof(OrderType), orderDto.Type)
                };

                bool areExistItems = true;
                List<OrderItem> orderItems = new List<OrderItem>();
                foreach (var orderItemDto in orderDto.OrderItems)
                {
                    var item = context.Items.Where(x => x.Name == orderItemDto.ItemName)
                        .FirstOrDefault();

                    var orderItem = new OrderItem()
                    {
                        Order = order,
                        Item = item,
                        Quantity = orderItemDto.Quantity
                    };
          
                    if(!IsValid(orderItemDto) || item == null || orderItems.Any(x => x == orderItem))
                    {
                        sb.AppendLine(FailureMessage);
                        areExistItems = false;
                        break;
                    }
                    
                    orderItems.Add(orderItem);
                }

                if(!areExistItems)
                {
                    continue;
                }         
                
                context.Orders.Add(order);
                context.SaveChanges();

                context.OrderItems.AddRange(orderItems);
                context.SaveChanges();

                sb.AppendLine($"Order for {order.Customer} on {date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
            }
            return sb.ToString().TrimEnd();
		}

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}