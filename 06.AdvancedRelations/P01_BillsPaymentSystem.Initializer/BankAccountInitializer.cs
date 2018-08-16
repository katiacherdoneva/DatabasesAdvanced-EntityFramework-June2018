using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Initializer
{
    public class BankAccountInitializer
    {
        public static BankAccount[] GetBankAccounts()
        {
            BankAccount[] bankAccounts = new BankAccount[]
            {
                new BankAccount() {BankName = "Crazy Bank", SwiftCode = "CRM"},
                new BankAccount() {BankName = "Lazy Bank", SwiftCode = "LZB"},
                new BankAccount() {BankName = "Sweet Bank", SwiftCode = "SWT"}
            };

            return bankAccounts;
        }
    }
}
