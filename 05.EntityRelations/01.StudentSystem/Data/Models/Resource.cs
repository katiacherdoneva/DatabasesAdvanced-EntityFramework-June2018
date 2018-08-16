using System;
using System.ComponentModel.DataAnnotations;
using P01_StudentSystem.Data.Models.Enum;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public string Url { get; set; }

        [Required]
        public ResourceType ResourceType { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
