using Application.Domain.Services;
using System;

namespace Application.Features
{
    public class TransferMoney
    {
        private ITransferService transferService;

        public TransferMoney(ITransferService withdrawService)
        {
            this.transferService = withdrawService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            this.transferService.TransferMoney(fromAccountId, toAccountId, amount);
        }
    }
}
