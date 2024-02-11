using System;
using Application.Attributes;

namespace Application.Domain
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public void PaidInToAccount(decimal amount)
        {
            Balance += amount;
            PaidIn += amount;
        }

        public void TransferByAccount(decimal amount)
        {
            this.ValidateTransferAmount(amount);

            Balance -= amount;
            Withdrawn -= amount; // ??
        }

        public void Withdraw(decimal amount)
        {
            this.ValidateWithdrawAmount(amount);

            Balance -= amount;
            Withdrawn += amount;
        }
    }
}
