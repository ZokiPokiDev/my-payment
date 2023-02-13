using Domain.Entities;
using Domain.Enums;
using UMG.DeveloperTest.Repositories;

namespace UMG.DeveloperTest.Tests.Fakers;

public class FakeAccountRepository : IAccountRepository
{
    private AllowedPaymentSchemes _allowedPaymentSchemes { get; }
    private decimal _balance { get; }

    public FakeAccountRepository(AllowedPaymentSchemes allowedPaymentSchemes, decimal balance)
    {
        _allowedPaymentSchemes = allowedPaymentSchemes;
        _balance = balance;
    }

    public Account? GetAccount(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            return null;
        }

        return new Account()
        {
            AccountNumber = accountNumber,
            AllowedPaymentSchemes = _allowedPaymentSchemes,
            Balance = _balance,
            Status = AccountStatus.Live,
        };
    }

    public void UpdateAccount(Account account)
    {
        return;
    }
}
