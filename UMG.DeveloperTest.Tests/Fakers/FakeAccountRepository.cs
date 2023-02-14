using Domain.Entities;
using Domain.Enums;
using Moq;
using UMG.DeveloperTest.Repositories;

namespace UMG.DeveloperTest.Tests.Fakers;

public class FakeAccountRepository
{
    public static Account? GetAccount(string accountNumber, AllowedPaymentSchemes allowedPaymentSchemes, decimal balance)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            return null;
        }

        return new Account()
        {
            AccountNumber = accountNumber,
            AllowedPaymentSchemes = allowedPaymentSchemes,
            Balance = balance,
            Status = AccountStatus.Live,
        };
    }
}
