using Domain.Enums;
using UMG.DeveloperTest.Requests;

namespace UMG.DeveloperTest.Tests.Fakers;

public class FakeMakePaymentRequest
{
    public MakePaymentRequest CreateFakeMakePaymentRequest(PaymentScheme paymentScheme, decimal amount)
    {
        var rand = new Random();

        return new MakePaymentRequest()
        {
            CreditorAccountNumber = rand.Next(1000, 10000).ToString(),
            Amount = amount,
            DebtorAccountNumber = rand.Next(100, 3000).ToString(),
            PaymentDate = DateTime.Now,
            PaymentScheme = paymentScheme,
        };
    }

    public MakePaymentRequest CreateFakeMakePaymentRequest(PaymentScheme paymentScheme, decimal amount, string debtorAccountNumber)
    {
        var rand = new Random();

        return new MakePaymentRequest()
        {
            CreditorAccountNumber = rand.Next(1000, 10000).ToString(),
            Amount = amount,
            DebtorAccountNumber = debtorAccountNumber,
            PaymentDate = DateTime.Now,
            PaymentScheme = paymentScheme,
        };
    }
}
