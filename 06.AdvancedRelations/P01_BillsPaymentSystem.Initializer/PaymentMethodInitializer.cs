using P01_BillsPaymentSystem.Data.Models;
using P01_BillsPaymentSystem.Data.Models.Enum;
using System;
using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Initializer
{
    public class PaymentMethodInitializer
    {
        public static PaymentMethod[] GetPaymentMethods()
        {
            PaymentMethod[] paymentMethods = new PaymentMethod[]
            {
                new PaymentMethod() {UserId = 1, BankAccountId = 1, Type = PaymentMethodType.BankAccount},
                new PaymentMethod() {UserId = 1, CreditCardId = 1, Type = PaymentMethodType.CreditCard},
                new PaymentMethod() {UserId = 2, CreditCardId = 2, Type = PaymentMethodType.CreditCard}
            };

            return paymentMethods;
        }
    }
}
