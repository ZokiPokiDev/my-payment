using Domain.Enums;
using Moq;
using UMG.DeveloperTest.Repositories;
using UMG.DeveloperTest.Requests;
using UMG.DeveloperTest.Services;
using UMG.DeveloperTest.Tests.Fakers;

namespace UMG.DeveloperTest.Tests;

public class TestPaymentService
{
    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    public void TestMakePaymentSuccessWithAciveAccount(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var repositoryMock = new Mock<IAccountRepository>();
        var rand = new Random();

        decimal amount = rand.Next(100, 1000);
        string accountNumber = rand.Next(1000, 10000).ToString();

        repositoryMock
            .Setup(r => r.GetAccount(accountNumber))
            .Returns(FakeAccountRepository.GetAccount(accountNumber, allowedPaymentSchemes, amount));
        var paymentService = new PaymentService(repositoryMock.Object);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount - 100, accountNumber);

        var result = paymentService.MakePayment(request);

        repositoryMock.Verify(r => r.GetAccount(accountNumber));
        Assert.True(result.Success);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    public void TestMakePaymentSuccessWithBackupAccount(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var repositoryMock = new Mock<IAccountRepository>();
        var rand = new Random();

        decimal amount = rand.Next(100, 1000);
        string accountNumber = rand.Next(1000, 10000).ToString();

        repositoryMock
            .Setup(r => r.GetAccount(accountNumber))
            .Returns(FakeAccountRepository.GetAccount(accountNumber, allowedPaymentSchemes, amount));
        var paymentService = new PaymentService(PaymentService.BACKUP_STORE_TYPE, repositoryMock.Object);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount - 100, accountNumber);

        var result = paymentService.MakePayment(request);

        repositoryMock.Verify(r => r.GetAccount(accountNumber));
        Assert.True(result.Success);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    public void TestNegativeBalance(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var repositoryMock = new Mock<IAccountRepository>();
        var rand = new Random();

        decimal amount = rand.Next(100, 1000);
        string accountNumber = rand.Next(1000, 10000).ToString();

        repositoryMock
            .Setup(r => r.GetAccount(accountNumber))
            .Returns(FakeAccountRepository.GetAccount(accountNumber, allowedPaymentSchemes, amount));
        var paymentService = new PaymentService(repositoryMock.Object);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount + 100, accountNumber);

        var result = paymentService.MakePayment(request);

        Assert.False(result.Success);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    public void TestNullAccount(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var repositoryMock = new Mock<IAccountRepository>();
        var rand = new Random();

        decimal amount = rand.Next(100, 1000);
        string accountNumber = string.Empty;

        repositoryMock
            .Setup(r => r.GetAccount(accountNumber))
            .Returns(FakeAccountRepository.GetAccount(accountNumber, allowedPaymentSchemes, amount));
        var paymentService = new PaymentService(repositoryMock.Object);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount - 100, accountNumber);

        var result = paymentService.MakePayment(request);

        Assert.False(result.Success);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments)]
    public void TestMakePaymentNotAllowedState(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var repositoryMock = new Mock<IAccountRepository>();
        var rand = new Random();

        decimal amount = rand.Next(100, 1000);
        string accountNumber = rand.Next(1000, 10000).ToString();

        repositoryMock
            .Setup(r => r.GetAccount(accountNumber))
            .Returns(FakeAccountRepository.GetAccount(accountNumber, allowedPaymentSchemes, amount));
        var paymentService = new PaymentService(repositoryMock.Object);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount - 100, accountNumber);

        var result = paymentService.MakePayment(request);

        Assert.False(result.Success);
    }
}
