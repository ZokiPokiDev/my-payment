using Domain.Enums;
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
        var rand = new Random();
        decimal amount = rand.Next(100, 1000);

        var fakeAccountRepository = new FakeAccountRepository(allowedPaymentSchemes, amount);
        var paymentService = new PaymentService(fakeAccountRepository);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount);

        var result = paymentService.MakePayment(request);

        Assert.True(result.Success);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    public void TestMakePaymentSuccessWithBackupAccount(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var rand = new Random();
        decimal amount = rand.Next(100, 1000);

        var fakeAccountRepository = new FakeAccountRepository(allowedPaymentSchemes, amount);
        var paymentService = new PaymentService(PaymentService.BACKUP_STORE_TYPE, fakeAccountRepository);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount - 100);

        var result = paymentService.MakePayment(request);

        Assert.True(result.Success);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
    public void TestNegativeBalance(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var rand = new Random();
        decimal amount = rand.Next(100, 1000);

        var fakeAccountRepository = new FakeAccountRepository(allowedPaymentSchemes, amount);
        var paymentService = new PaymentService(fakeAccountRepository);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount + 100);

        var result = paymentService.MakePayment(request);

        Assert.False(result.Success);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
    public void TestNullAccount(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var rand = new Random();
        decimal amount = rand.Next(100, 1000);

        var fakeAccountRepository = new FakeAccountRepository(allowedPaymentSchemes, amount);
        var paymentService = new PaymentService(fakeAccountRepository);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount + 100, string.Empty);

        var result = paymentService.MakePayment(request);

        Assert.False(result.Success);
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs)]
    [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments)]
    public void TestMakePaymentNotAllowedState(AllowedPaymentSchemes allowedPaymentSchemes, PaymentScheme paymentScheme)
    {
        var rand = new Random();
        decimal amount = rand.Next(100, 1000);

        var fakeAccountRepository = new FakeAccountRepository(allowedPaymentSchemes, amount);
        var paymentService = new PaymentService(fakeAccountRepository);
        MakePaymentRequest request = new FakeMakePaymentRequest().CreateFakeMakePaymentRequest(paymentScheme, amount);

        var result = paymentService.MakePayment(request);

        Assert.False(result.Success);
    }
}
