﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetClinic.DataProcessor.Dto
{
    public class PassportDto
    {
        [Key]
        [StringLength(10)]
        [RegularExpression("^([A-Za-z]{7}[0-9]{3})$")]
        public string SerialNumber { get; set; }

        [Required]
        [RegularExpression(@"^(\+359|0)[0-9]{9}$")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string OwnerName { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }
    }
}
