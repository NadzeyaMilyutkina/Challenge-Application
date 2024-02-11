using System;

namespace Application.Domain.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsBalanceValidForTransfer(decimal balance, decimal amount)
        {
            if (balance - amount < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            return balance - amount != 0m;
        }

        public bool IsBalanceValidForWithdraw(decimal balance, decimal amount)
        {
            if (balance - amount < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make withdraw");
            }

            return balance - amount != 0m;
        }

        public void IsPaidInLimitReached(decimal paidIn)
        {
            if (paidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }
        }
    }
}
