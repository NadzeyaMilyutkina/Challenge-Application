using System;
using Application.Domain;

namespace Application.Attributes
{
    public static class ValidateAmountExtension
    {
        /// <summary>
        /// The <see cref="ValidateTransferAmount"/> method validates an ability of an <param name="amount"/>
        /// to be transferred by <param name="account"/>.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="amount"></param>
        /// <exception cref="InvalidOperationException">Insufficient funds to make transfer</exception>
        public static void ValidateTransferAmount(this Account account, decimal amount)
        {
            if (account.Balance - amount < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
        }

        /// <summary>
        /// The <see cref="ValidateWithdrawAmount"/> method validates an ability of an <param name="amount"/>
        /// to be withdrawn by <param name="account"/>.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="amount"></param>
        /// <exception cref="InvalidOperationException">Insufficient funds to make withdraw</exception>
        public static void ValidateWithdrawAmount(this Account account, decimal amount)
        {
            if (account.Balance - amount < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make withdraw");
            }
        }

        /// <summary>
        /// This <see cref="ValidatePaidInLimitReached"/> method validates the <param name="amount"/> on the correspondence to the Limit.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="amount"></param>
        /// <exception cref="InvalidOperationException">'Account pay in limit reached'</exception>
        public static void ValidatePaidInLimitReached(this Account account, decimal amount)
        {
            if (account.PaidIn + amount > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }
        }
    }
}
