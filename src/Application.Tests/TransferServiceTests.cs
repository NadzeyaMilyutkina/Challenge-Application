using Application.DataAccess;
using Application.Domain;
using Application.Domain.Services;
using Moq;
using Xunit;

namespace Application.Tests
{
    public class TransferServiceTests
    {
        private Mock<INotificationService> notificationServiceMock;
        private Mock<IAccountRepository> accountRepositoryMock;
        private readonly Guid transferTo = Guid.NewGuid();
        private readonly Guid transferBy = Guid.NewGuid();

        public TransferServiceTests()
        {
            this.notificationServiceMock = new Mock<INotificationService>();
            this.accountRepositoryMock = new Mock<IAccountRepository>();
        }

        // need to ask about withdraw (as it becomes negative)
        [Fact]
        public void TransferMoney_NormalFlow_MoneyWasTransfered()
        {
            StubAccounts(1000m, 500m);
            ITransferService transferService =
                new TransferService(accountRepositoryMock.Object, notificationServiceMock.Object);

            transferService.TransferMoney(transferBy, transferTo, 250);

            accountRepositoryMock.Verify(m => m.GetAccountById(transferTo), Times.Once);
            accountRepositoryMock.Verify(m => m.GetAccountById(transferBy), Times.Once);
            accountRepositoryMock.Verify(m => m.Update(It.IsAny<Account>()), Times.Exactly(2));
        }

        [Fact]
        public void TransferMoney_FundsLow_NotifyFundsLowMessageWasSent()
        {
            StubAccounts(700m, 500m);
            notificationServiceMock.Setup(x => x.NotifyApproachingPayInLimit(It.IsAny<string>()));

            ITransferService transferService =
                new TransferService(accountRepositoryMock.Object, notificationServiceMock.Object);

            transferService.TransferMoney(transferBy, transferTo, 250);

            notificationServiceMock.Verify(m => m.NotifyFundsLow(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void TransferMoney_PayInLimitReached_InvalidOperationException()
        {
            StubAccounts(4400m, 3600m);
            ITransferService transferService =
                new TransferService(accountRepositoryMock.Object, notificationServiceMock.Object);

            var exception =
                Assert.Throws<InvalidOperationException>(() =>
                    transferService.TransferMoney(transferBy, transferTo, 4100));
            Assert.Equal($"Account pay in limit reached", exception.Message);
        }

        [Fact]
        public void TransferMoney_PayInLimit_NotifyApproachingPayInLimitMessageWasSent()
        {
            StubAccounts(3700m, 600m);

            ITransferService transferService =
                new TransferService(accountRepositoryMock.Object, notificationServiceMock.Object);

            transferService.TransferMoney(transferBy, transferTo, 3600);

            notificationServiceMock.Verify(m => m.NotifyApproachingPayInLimit(It.IsAny<string>()), Times.Once);
        }

        private void StubAccounts(decimal byBalance, decimal toBalance, string email = "test@test.test")
        {
            var user = new User { Email = email };

            accountRepositoryMock
                .SetupSequence(x => x.GetAccountById(It.IsAny<Guid>()))
                .Returns(new Account { Id = transferBy, User = user, Balance = byBalance })
                .Returns(new Account { Id = transferTo, User = user, Balance = toBalance });
        }
    }
}