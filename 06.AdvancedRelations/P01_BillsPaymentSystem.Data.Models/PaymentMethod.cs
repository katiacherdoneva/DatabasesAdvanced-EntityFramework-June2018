using System;
using P01_BillsPaymentSystem.Data.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using P01_BillsPaymentSystem.Data.Models.ValidationAttributes;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("BankAccount")]
        [Xor(nameof(CreditCardId))]
        public int? BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        [ForeignKey("CreditCard")]
        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }

        [Required]
        public PaymentMethodType Type { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
