using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dto
{
    [XmlType("Procedure")]
    public class ProcedureDto
    {
        [StringLength(10)]
        [RegularExpression("^([A-Za-z]{7}[0-9]{3})$")]
        [XmlElement("Animal")]
        public string SerialNumberAnimal { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        [XmlElement("Vet")]
        public string VetName { get; set; }

        [Required]
        [XmlElement()]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public AnimalAidDtoForProcedure[] AnimalAids { get; set; }
    }
}
