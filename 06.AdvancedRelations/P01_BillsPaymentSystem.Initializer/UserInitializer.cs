using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Initializer
{
    public class UserInitializer
    {
        public static User[] GetUsers()
        {
            User[] users = new User[]
            {
                new User() {FirstName = "Gosho", LastName = "Ivanov", Email = "gogo@softuni.bg", Password = "gogo8978"},
                new User() {FirstName = "Ivan", LastName = "Petrov", Email = "ivancho@softuni.bg", Password = "iviviv"},
                new User() {FirstName = "Petyr", LastName = "Ivanov", Email = "petyriv@softuni.bg", Password = "ivanow123"}
            };

            return users;
        }
    }
}
