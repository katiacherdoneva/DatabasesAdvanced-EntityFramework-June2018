using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public class Serializer
	{
		public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
		{
            var employee = context.Employees
                .ToArray()
                .Where(x => x.Name == employeeName)
                .Select(x => new
                {
                    Name = x.Name,
                    Orders = x.Orders.Where(y => y.Type.ToString() == orderType)
                              .Select(o => new
                              {
                                  Customer = o.Customer,
                                  Items = o.OrderItems.Select(i => new
                                  {
                                      Name = i.Item.Name,
                                      Price = i.Item.Price,
                                      Quantity = i.Quantity
                                  }).ToArray(),
                                  TotalPrice = o.TotalPrice
                              })
                              .OrderByDescending(t => t.TotalPrice)
                              .ThenByDescending(z => z.Items.Length)
                              .ToArray(),
                    TotalMade = x.Orders.Where(s => s.Type.ToString() == orderType)
                                             .Sum(z => z.TotalPrice)
                }).FirstOrDefault();


            var serializerJson = JsonConvert.SerializeObject(employee, Newtonsoft.Json.Formatting.Indented);
            return serializerJson;
		}

		public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
		{
            var categoriesArray = categoriesString.Split(',');

            CategoryDto[] categories = context.Categories
                .Where(x => categoriesArray.Any(s => s == x.Name))
                .Select(c => new CategoryDto
                {
                    Name = c.Name,
                    MostPopularItem = c.Items
                    .Select(x => new MostPopularItemDto
                    {
                        Name = x.Name,
                        TimesSold = x.OrderItems.Sum(s => s.Quantity),
                        TotalMade = x.OrderItems.Sum(s => s.Quantity * s.Item.Price)
                    })
                    .OrderByDescending(y => y.TotalMade)
                    .ThenByDescending(y => y.TimesSold)
                    .FirstOrDefault()
                })
                .OrderByDescending(x => x.MostPopularItem.TotalMade)
                .ThenByDescending(x => x.MostPopularItem.TimesSold)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));
            serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            return sb.ToString();

        }
	}
}