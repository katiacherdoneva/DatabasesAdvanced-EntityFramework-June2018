using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Initializer
{
    public class CreditCardInitializer
    {
        public static CreditCard[] GetCreditCards()
        {
            CreditCard[] creditCards = new CreditCard[]
            {
                new CreditCard() {ExpirationDate = DateTime.Now.AddDays(-15)},
                new CreditCard() {ExpirationDate = DateTime.Now.AddMonths(-1)},
                new CreditCard() {ExpirationDate = DateTime.Now.AddDays(-10)}
            };

            return creditCards;
        }
    }
}
