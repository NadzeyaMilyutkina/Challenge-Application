using System;

namespace Application.Domain.Services
{
    public interface ITransferService
    {
        void TransferMoney(Guid fromAccountId, Guid toAccountId, decimal amount);

    }
}