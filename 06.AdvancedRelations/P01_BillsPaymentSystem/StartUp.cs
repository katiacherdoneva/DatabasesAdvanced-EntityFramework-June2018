using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using P01_BillsPaymentSystem.Initializer;
using System;
using System.Linq;

namespace P01_BillsPaymentSystem
{
    public class StartUp
    {
        static void Main()
        {
            //using (BillsPaymentSystemContext context = new BillsPaymentSystemContext())
            //{
            //    context.Database.EnsureDeleted();
            //    context.Database.EnsureCreated();
            //}
            //using (BillsPaymentSystemContext context = new BillsPaymentSystemContext())
            //{
            //    Initialize.Seed(context);
            //}

            using (BillsPaymentSystemContext context = new BillsPaymentSystemContext())
            {
                var user = GetUser(context);
                GetInfo(user);
                PayBills(user, 300);
            }
        }

        private static void PayBills(User user, decimal amount)
        {
            var bankAccountTotals = user.PaymentMethods.Where(x => x.BankAccount != null).Sum(x => x.BankAccount.Balance);
            var creditCartTotals = user.PaymentMethods.Where(x => x.BankAccount != null).Sum(x => x.BankAccount.Balance);

            var totalSum = bankAccountTotals + creditCartTotals;

            if (totalSum >= amount)
            {
                amount = WithDrawBankAccounts(user, amount);
                if(amount > 0)
                {
                    WithDrawCreditCards(user, amount);
                }
                
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
            }

        }

        private static void WithDrawCreditCards(User user, decimal amount)
        {
            var creditCards = user.PaymentMethods
                    .Where(x => x.CreditCard != null)
                    .Select(x => x.CreditCard)
                    .OrderBy(x => x.CreditCardId)
                    .ToArray();

            foreach (var c in creditCards)
            {
                if (amount >= c.LimitLeft)
                {
                    amount -= c.LimitLeft;
                    c.WithDraw(c.LimitLeft);
                }
                else
                {
                    c.WithDraw(amount);
                    amount = 0;
                }

                if (amount == 0)
                {
                    return;
                }
            }
        }

        private static decimal WithDrawBankAccounts(User user, decimal amount)
        {
            var bankAccounts = user.PaymentMethods
                    .Where(x => x.BankAccount != null)
                    .Select(x => x.BankAccount)
                    .OrderBy(x => x.BankAccountId)
                    .ToArray();

            foreach (var b in bankAccounts)
            {
                if (amount >= b.Balance)
                {
                    amount -= b.Balance;
                    b.WithDraw(b.Balance);
                }
                else
                {
                    b.WithDraw(amount);
                    amount = 0;
                }

                if (amount == 0)
                {
                    return amount;
                }
            }
            return amount;
        }

        private static void GetInfo(User user)
        {
            Console.WriteLine($"User: {user.FirstName} {user.LastName}");

            Console.WriteLine("Bank Accounts:");
            var bankAccounts = user.PaymentMethods.Where(x => x.BankAccount != null).Select(x => x.BankAccount);

            foreach (var b in bankAccounts)
            {
                Console.WriteLine($"-- ID: {b.BankAccountId}"
                        + Environment.NewLine + $"--- Balance: {b.Balance:F2}"
                        + Environment.NewLine + $"--- Bank: {b.BankName}"
                        + Environment.NewLine + $"--- SWIFT: {b.SwiftCode}");
            }

            Console.WriteLine("Credit Cards:");
            var creditCards = user.PaymentMethods.Where(x => x.CreditCard != null).Select(x => x.CreditCard);

            foreach (var c in creditCards)
            {
                Console.WriteLine($"-- ID: {c.CreditCardId}"
                        + Environment.NewLine + $"--- Limit: {c.Limit:F2}"
                        + Environment.NewLine + $"--- Money Owed: {c.MoneyOwned:F2}"
                        + Environment.NewLine + $"--- Limit Left:: {c.LimitLeft:F2}"
                        + Environment.NewLine + $"--- Expiration Date: {c.ExpirationDate.ToString("yyyy/MM")}");
            }
        }

        private static User GetUser(BillsPaymentSystemContext context)
        {
            int userId = int.Parse(Console.ReadLine());

            User user = null;

            while (true)
            {
                user = context.Users
                     .Where(x => x.UserId == userId)
                     .Include(x => x.PaymentMethods)
                     .ThenInclude(x => x.BankAccount)
                     .Include(x => x.PaymentMethods)
                     .ThenInclude(x => x.CreditCard)
                     .FirstOrDefault();

                if (user == null)
                {
                    Console.WriteLine($"User with id {userId} not found!");
                    userId = int.Parse(Console.ReadLine());
                    continue;
                }

                break;
            }

            return user;
        }
    }
}
