using Application.Domain.Services;
using System;

namespace Application.Features
{
    public class WithdrawMoney
    {
        private IWithdrawService withdrawService;

        public WithdrawMoney(IWithdrawService withdrawService)
        {
            this.withdrawService = withdrawService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            this.withdrawService.WithdrawMoney(fromAccountId, amount);
        }
    }
}
