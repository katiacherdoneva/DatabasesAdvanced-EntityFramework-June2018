using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using P01_StudentSystem.Data.Models.Enum;
using System;
using System.Linq;

namespace P01_StudentSystem
{
    public class StartUp
    {
        static void Main()
        {
            FillsTheDatabase();

            ReadInformationAboutCoursesAndStudent();
        }

        private static void ReadInformationAboutCoursesAndStudent()
        {
            using (StudentSystemContext context = new StudentSystemContext())
            {
                Console.WriteLine("Students: ");
                foreach (var s in context.Students.ToArray())
                {
                    Console.WriteLine($"{s.StudentId} {s.Name} {s.PhoneNumber} {s.RegisteredOn} {s.Birthday}");
                }

                Console.WriteLine("Courses: ");
                foreach (var c in context.Courses.ToArray())
                {
                    Console.WriteLine($"{c.CourseId} {c.Name} {c.Description} {c.StartDate} {c.EndDate} {c.Price}");
                }
            }
        }

        private static void FillsTheDatabase()
        {
            using (StudentSystemContext context = new StudentSystemContext())
            {
                var student = new Student
                {
                    Name = "Ivan",
                    RegisteredOn = DateTime.ParseExact("03/02/2017", "dd/MM/yyyy",
                          System.Globalization.CultureInfo.InvariantCulture)
                };

                var course = new Course
                {
                    Name = "Db Basic",
                    StartDate = DateTime.ParseExact("12/03/2017", "dd/MM/yyyy",
                          System.Globalization.CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact("28/05/2017", "dd/MM/yyyy",
                          System.Globalization.CultureInfo.InvariantCulture),
                    Price = (decimal)330.00
                };

                var homework = new Homework
                {
                    Content = "Exercises",
                    ContentType = (ContentType)Enum.Parse(typeof(ContentType), "Zip"),
                    SubmissionTime = DateTime.ParseExact("11/05/2017", "dd/MM/yyyy",
                          System.Globalization.CultureInfo.InvariantCulture),
                    StudentId = 1,
                    CourseId = 1
                };

                context.Students.Add(student);
                context.Courses.Add(course);
                context.HomeworkSubmissions.Add(homework);

                context.SaveChanges();
            }
        }
    }
}
