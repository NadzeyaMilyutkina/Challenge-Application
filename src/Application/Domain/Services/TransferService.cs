using System;
using Application.Attributes;
using Application.DataAccess;

namespace Application.Domain.Services
{
    public class TransferService : ITransferService
    {
        private readonly IAccountRepository accountRepository;
        private readonly INotificationService notificationService;

        public TransferService(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void TransferMoney(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);

            from.ValidateTransferAmount(amount);

            if (from.Balance - amount < 500m)
            {
                this.notificationService.NotifyFundsLow(to.User.Email);
            }

            var paidIn = to.PaidIn + amount;
            to.ValidatePaidInLimitReached(amount);

            if (Account.PayInLimit - paidIn < 500m)
            {
                this.notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }

            from.TransferByAccount(amount);

            to.PaidInToAccount(amount);

            this.accountRepository.Update(from);
            this.accountRepository.Update(to);
        }
    }
}
