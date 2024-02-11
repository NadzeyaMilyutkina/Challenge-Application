using Application.DataAccess;
using Application.Domain;
using Application.Domain.Services;
using Moq;
using Xunit;

namespace Application.Tests
{
    public class WithdrawServiceTests
    {
        private Mock<INotificationService> notificationServiceMock;
        private Mock<IAccountRepository> accountRepositoryMock;
        private IWithdrawService withdrawService;
        private readonly Guid withdrawnBy = Guid.NewGuid();

        public WithdrawServiceTests()
        {
            this.notificationServiceMock = new Mock<INotificationService>();
            this.accountRepositoryMock = new Mock<IAccountRepository>();
            this.withdrawService = new WithdrawService(accountRepositoryMock.Object, notificationServiceMock.Object);
        }

        // need to ask about withdraw (as it becomes negative)
        [Fact]
        public void WithdrawMoney_NormalFlow_MoneyWasWithdrawn()
        {
            StubAccount(1000m);
            IWithdrawService withdrawService =
                new WithdrawService(accountRepositoryMock.Object, notificationServiceMock.Object);

            withdrawService.WithdrawMoney(withdrawnBy, 250);

            accountRepositoryMock.Verify(m => m.GetAccountById(withdrawnBy), Times.Once);
            accountRepositoryMock.Verify(m => m.Update(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void WithdrawMoney_FundsLow_NotifyFundsLow()
        {
            StubAccount(700m);

            withdrawService.WithdrawMoney(withdrawnBy, 700m);

            notificationServiceMock.Verify(m => m.NotifyFundsLow(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void WithdrawMoney_InsufficientFunds_InvalidOperationException()
        {
            StubAccount(700m);

            var exception =
                Assert.Throws<InvalidOperationException>(() =>
                    withdrawService.WithdrawMoney(withdrawnBy, 800m));
            Assert.Equal($"Insufficient funds to make withdraw", exception.Message);
        }

        private void StubAccount(decimal byBalance, string email = "test@test.test")
        {
            var user = new User { Email = email };

            accountRepositoryMock
                .Setup(x => x.GetAccountById(It.IsAny<Guid>()))
                .Returns(new Account { Id = withdrawnBy, User = user, Balance = byBalance });
        }
    }
}
