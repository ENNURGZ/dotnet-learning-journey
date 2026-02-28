using System;
using Moq;
using NUnit.Framework;
using MockTesting;

namespace MockTesting.Tests
{
    [TestFixture]
    public class TransactionProcessorTests
    {
        private Mock<IPermissionService> permissionMock;
        private Mock<IAccountService> accountMock;
        private Mock<ITransactionService> transactionMock;
        private Mock<ILogger> loggerMock;

        private TransactionProcessor processor;

        [SetUp]
        public void Setup()
        {
            this.permissionMock = new Mock<IPermissionService>();
            accountMock = new Mock<IAccountService>();
            transactionMock = new Mock<ITransactionService>();
            loggerMock = new Mock<ILogger>();

            processor = new TransactionProcessor(
                permissionMock.Object,
                accountMock.Object,
                transactionMock.Object,
                loggerMock.Object);
        }

        #region Constructor Tests

        [Test]
        public void Constructor_NullDependencies_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new TransactionProcessor(null!, accountMock.Object, transactionMock.Object, loggerMock.Object));

            Assert.Throws<ArgumentNullException>(() =>
                new TransactionProcessor(permissionMock.Object, null!, transactionMock.Object, loggerMock.Object));

            Assert.Throws<ArgumentNullException>(() =>
                new TransactionProcessor(permissionMock.Object, accountMock.Object, null!, loggerMock.Object));

            Assert.Throws<ArgumentNullException>(() =>
                new TransactionProcessor(permissionMock.Object, accountMock.Object, transactionMock.Object, null!));
        }

        #endregion

        #region Successful Scenarios

        [Test]
        public void ProcessTransfer_WhenAllConditionsMet_ShouldSucceed()
        {
            permissionMock.Setup(p => p.HasTransferPermission(1)).Returns(true);
            accountMock.Setup(a => a.GetBalance(1)).Returns(1000);

            var result = processor.ProcessTransfer(1, 2, 100);

            Assert.That(result, Is.True);

            transactionMock.Verify(t => t.Transfer(1, 2, 100), Times.Once);
            loggerMock.Verify(l => l.Log(It.Is<string>(s =>
                s.Contains("started") || s.Contains("completed successfully"))),
                Times.AtLeast(2));
        }

        [Test]
        public void ProcessTransfer_WithMaximumValidAmount_ShouldSucceed()
        {
            permissionMock.Setup(p => p.HasTransferPermission(1)).Returns(true);
            accountMock.Setup(a => a.GetBalance(1)).Returns(decimal.MaxValue);

            var result = processor.ProcessTransfer(1, 2, decimal.MaxValue);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ProcessTransfer_WithMinimumValidAmount_ShouldSucceed()
        {
            permissionMock.Setup(p => p.HasTransferPermission(1)).Returns(true);
            accountMock.Setup(a => a.GetBalance(1)).Returns(1);

            var result = processor.ProcessTransfer(1, 2, 0.01m);

            Assert.That(result, Is.True);
        }

        #endregion

        #region Permission Tests

        [Test]
        public void ProcessTransfer_WhenPermissionDenied_ShouldFail()
        {
            permissionMock.Setup(p => p.HasTransferPermission(1)).Returns(false);

            var result = processor.ProcessTransfer(1, 2, 100);

            Assert.That(result, Is.False);

            transactionMock.Verify(t => t.Transfer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
            loggerMock.Verify(l => l.Log(It.Is<string>(s => s.Contains("Permission denied"))), Times.Once);
        }

        #endregion

        #region Balance Tests

        [Test]
        public void ProcessTransfer_WhenInsufficientBalance_ShouldFail()
        {
            permissionMock.Setup(p => p.HasTransferPermission(1)).Returns(true);
            accountMock.Setup(a => a.GetBalance(1)).Returns(50);

            var result = processor.ProcessTransfer(1, 2, 100);

            Assert.That(result, Is.False);
            transactionMock.Verify(t => t.Transfer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
            loggerMock.Verify(l => l.Log(It.Is<string>(s => s.Contains("Insufficient funds"))), Times.Once);
        }

        #endregion

        #region Exception Handling

        [Test]
        public void ProcessTransfer_WhenTransactionThrowsException_ShouldReturnFalse()
        {
            permissionMock.Setup(p => p.HasTransferPermission(1)).Returns(true);
            accountMock.Setup(a => a.GetBalance(1)).Returns(1000);
            transactionMock.Setup(t => t.Transfer(1, 2, 100))
                           .Throws(new Exception("DB error"));

            var result = processor.ProcessTransfer(1, 2, 100);

            Assert.That(result, Is.False);

            loggerMock.Verify(l => l.Log(It.Is<string>(s => s.Contains("failed with error: DB error"))), Times.Once);
        }

        #endregion

        #region Input Validation

        [Test]
        public void ProcessTransfer_InvalidFromUserId_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                processor.ProcessTransfer(0, 2, 100));
        }

        [Test]
        public void ProcessTransfer_InvalidToUserId_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                processor.ProcessTransfer(1, 0, 100));
        }

        [Test]
        public void ProcessTransfer_SameUserIds_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                processor.ProcessTransfer(1, 1, 100));
        }

        [Test]
        public void ProcessTransfer_InvalidAmount_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() =>
                processor.ProcessTransfer(1, 2, 0));
        }

        #endregion

        #region Transaction ID & Logging

        [Test]
        public void ProcessTransfer_ShouldGenerateUniqueTransactionId()
        {
            permissionMock.Setup(p => p.HasTransferPermission(It.IsAny<int>())).Returns(true);
            accountMock.Setup(a => a.GetBalance(It.IsAny<int>())).Returns(1000);

            processor.ProcessTransfer(1, 2, 10);
            processor.ProcessTransfer(1, 2, 10);

            loggerMock.Verify(l => l.Log(It.Is<string>(s =>
    s.StartsWith("Transaction "))),
    Times.AtLeast(2));
        }

        #endregion
    }
}
