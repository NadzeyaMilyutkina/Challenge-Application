using System;
using Application.DataAccess;

namespace Application.Domain.Services
{
    public class WithdrawService : IWithdrawService
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawService(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void WithdrawMoney(Guid fromAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);

            if (from.Balance - amount == 0m)
            {
                this.notificationService.NotifyFundsLow(from.User.Email);
            }

            from.Withdraw(amount);
            this.accountRepository.Update(from);
        }
    }
}