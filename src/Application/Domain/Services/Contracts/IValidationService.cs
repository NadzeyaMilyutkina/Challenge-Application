namespace Application.Domain.Services
{
    public interface IValidationService
    {
        bool IsBalanceValidForTransfer(decimal balance, decimal amount);

        bool IsBalanceValidForWithdraw(decimal balance, decimal amount);

        void IsPaidInLimitReached(decimal paidIn);
    }
}