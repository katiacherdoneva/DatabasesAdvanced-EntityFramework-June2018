﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; }

        [Required]
        public decimal Balance { get; private set; }

        [Required]
        [StringLength(50)]
        public string BankName { get; set; }

        [Required]
        [StringLength(20)]
        public string SwiftCode { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if(amount > 0)
            {
                this.Balance += amount;
            }
        }

        public void WithDraw(decimal amount)
        {
            if(this.Balance - amount > 0)
            {
                this.Balance -= amount;
            }
        }
    }
}
