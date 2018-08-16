namespace PetClinic.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.Dto;

    public class Serializer
    {
    //    "OwnerName": "Ivan Ivanov",
    //"AnimalName": "Jessy",
    //"Age": 3,
    //"SerialNumber": "jessiii355",
    //"RegisteredOn": "05-11-2015"

        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var passport = context.Passports.Where(x => x.OwnerPhoneNumber == phoneNumber)
                .OrderBy(x => x.Animal.Age)
                .ThenBy(x => x.SerialNumber)
                .Select(x => new
                {
                    OwnerName = x.OwnerName,
                    AnimalName = x.Animal.Name,
                    Age = x.Animal.Age,
                    SerialNumber = x.SerialNumber,
                    RegisteredOn = x.RegistrationDate.ToString("dd-MM-yyyy")
                }).ToArray();

            var serializerJson = JsonConvert.SerializeObject(passport, Newtonsoft.Json.Formatting.Indented);
            return serializerJson;
        }

    //    <Procedure>
    //<Passport>acattee321</Passport>
    //<OwnerNumber>0887446123</OwnerNumber>
    //<DateTime>14-01-2016</DateTime>
    //<AnimalAids>
    //  <AnimalAid>
    //    <Name>Internal Deworming</Name>
    //    <Price>8.00</Price>
    //  </AnimalAid>
    //  <AnimalAid>
    //    <Name>Fecal Test</Name>
    //    <Price>7.50</Price>
    //  </AnimalAid>

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var procedures = context.Procedures
                .OrderBy(x => x.DateTime)
                .ThenBy(x => x.Animal.PassportSerialNumber)
                .Select(x => new ProcedureDtoExport
                {
                    PassportSerialNumber = x.Animal.PassportSerialNumber,
                    OwnerNumber = x.Animal.Passport.OwnerPhoneNumber,
                    DateTime = x.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    AnimalAids = x.ProcedureAnimalAids.Select(y => new AnimalAidDto
                    {
                        Name = y.AnimalAid.Name,
                        Price = y.AnimalAid.Price
                    }).ToArray(),
                    Cost = x.Cost
                }).ToArray();

            StringBuilder sb = new StringBuilder();
            var xmlNamespaces = new XmlSerializerNamespaces(new [] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(ProcedureDtoExport[]), new XmlRootAttribute("Procedures"));
            serializer.Serialize(new StringWriter(sb), procedures, xmlNamespaces);

            return sb.ToString();
        }
    }
}
