using System;

namespace Application.Domain.Services
{
    public interface IWithdrawService
    {
        void WithdrawMoney(Guid fromAccountId, decimal amount);
    }
}