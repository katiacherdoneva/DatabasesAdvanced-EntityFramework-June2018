namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using PetClinic.Data;
    using PetClinic.DataProcessor.Dto;
    using PetClinic.Models;

    public class Deserializer
    {

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserializedAnimalAid = JsonConvert.DeserializeObject<AnimalAidDto[]>(jsonString);

            List<AnimalAid> animalAids = new List<AnimalAid>();
            foreach (var animalAidDto in deserializedAnimalAid)
            {
                if (!IsValid(animalAidDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                bool isExistAnimalAid = animalAids.Any(x => x.Name == animalAidDto.Name);
                if (isExistAnimalAid)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animalAid = new AnimalAid()
                {
                    Name = animalAidDto.Name,
                    Price = animalAidDto.Price
                };

                animalAids.Add(animalAid);
                sb.AppendLine($"Record {animalAid.Name} successfully imported.");
            }

            context.AnimalAids.AddRange(animalAids);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var format = "dd-MM-yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var deserializedAnimal = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString, dateTimeConverter);

            List<Animal> animals = new List<Animal>();
            foreach (var animalDto in deserializedAnimal)
            {
                bool isExistsAnimal = animals.Any(x => x.Passport.SerialNumber == animalDto.Passport.SerialNumber);
                if (!IsValid(animalDto) || isExistsAnimal || !IsValid(animalDto.Passport))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var passport = new Passport()
                {
                    SerialNumber = animalDto.Passport.SerialNumber,
                    OwnerName = animalDto.Passport.OwnerName,
                    OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                    RegistrationDate = animalDto.Passport.RegistrationDate
                };

                context.Passports.Add(passport);
                context.SaveChanges();

                var animal = new Animal()
                {
                    Name = animalDto.Name,
                    Type = animalDto.Type,
                    Age = animalDto.Age,
                    Passport = passport
                };

                animals.Add(animal);
                sb.AppendLine($"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
            }
            context.Animals.AddRange(animals);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(VetDto[]), new XmlRootAttribute("Vets"));
            var deserializeVets = (VetDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Vet> vets = new List<Vet>();
            foreach (var vetDto in deserializeVets)
            {
                bool isExistsVet = vets.Any(x => x.PhoneNumber == vetDto.PhoneNumber);
                if(!IsValid(vetDto) || isExistsVet)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var vet = new Vet()
                {
                    Name = vetDto.Name,
                    Profession = vetDto.Profession,
                    Age = vetDto.Age,
                    PhoneNumber = vetDto.PhoneNumber
                };

                vets.Add(vet);
                sb.AppendLine($"Record {vet.Name} successfully imported.");
            }
            context.Vets.AddRange(vets);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));
            var deserializerProcedures = (ProcedureDto[])serializer.Deserialize(new StringReader(xmlString));

            foreach (var procedureDto in deserializerProcedures)
            {
                bool dateIsValid = DateTime
                    .TryParseExact(procedureDto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);
                if (!IsValid(procedureDto) || !dateIsValid)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var vet = context.Vets.Where(x => x.Name == procedureDto.VetName).FirstOrDefault();
                var animal = context.Animals.Where(x => x.Passport.SerialNumber == procedureDto.SerialNumberAnimal).FirstOrDefault();

                if (vet == null || animal == null || procedureDto.AnimalAids == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                List<AnimalAid> animalAids = new List<AnimalAid>();
                var allAidsExist = true;
                foreach (var animalAidDto in procedureDto.AnimalAids)
                {
                    var animalAid = context.AnimalAids.Where(x => x.Name == animalAidDto.Name).FirstOrDefault();
                    bool isExistsAnimalAid = animalAids.Any(x => x.Name == animalAid.Name);
                    if(animalAid == null || !IsValid(animalAidDto) || isExistsAnimalAid)
                    {
                        allAidsExist = false;
                        break;
                    }
                   
                    animalAids.Add(animalAid);
                }

                if (!allAidsExist)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var date = DateTime
                    .ParseExact(procedureDto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var procedure = new Procedure()
                {
                    Vet = vet,
                    Animal = animal,
                    DateTime = date                  
                };
                context.Procedures.Add(procedure);
                context.SaveChanges();

                List<ProcedureAnimalAid> procedureAnimalAids = new List<ProcedureAnimalAid>();
                foreach (var aa in animalAids)
                {
                    var procedureAnimalAid = new ProcedureAnimalAid()
                    {
                        Procedure = procedure,
                        AnimalAid = aa
                    };
                    procedureAnimalAids.Add(procedureAnimalAid);
                }
                context.ProceduresAnimalAids.AddRange(procedureAnimalAids);
                context.SaveChanges();

                sb.AppendLine("Record successfully imported.");
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
