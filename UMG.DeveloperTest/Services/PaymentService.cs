using Domain.Entities;
using Domain.Enums;
using System.Configuration;
using UMG.DeveloperTest.Repositories;
using UMG.DeveloperTest.Requests;
using UMG.DeveloperTest.Results;

namespace UMG.DeveloperTest.Services;

public class PaymentService : IPaymentService
{
    public const string DATA_STORE_TYPE_KEY = "DataStoreType";
    public const string BACKUP_STORE_TYPE = "Backup";

    protected string DataStoreType { get; }
    protected IAccountRepository AccountRepository { get; }
    protected MakePaymentResult Result { get; set; }

    private readonly bool _isBackupAccount;

    public PaymentService(IAccountRepository accountRepository)
    {
        DataStoreType = ConfigurationManager.AppSettings[DATA_STORE_TYPE_KEY];
        _isBackupAccount = IsBackupAccount(DataStoreType);
        AccountRepository = accountRepository;
        Result = new();
    }

    public PaymentService(string dataStoreType, IAccountRepository accountRepository)
    {
        DataStoreType = dataStoreType;
        _isBackupAccount = IsBackupAccount(DataStoreType);
        AccountRepository = accountRepository;
        Result = new();
    }

    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {
        var account = AccountRepository.GetAccount(request.DebtorAccountNumber);

        if (account == null)
        {
            Result.Success = false;
            return Result;
        }

        Result = ProcessPayment(account, request);
        UpdateAccountBalance(account, request, Result, _isBackupAccount);

        return Result;
    }

    private bool IsBackupAccount(string dataStoreType)
    {
        return dataStoreType == BACKUP_STORE_TYPE;
    }

    private MakePaymentResult ProcessPayment(Account account, MakePaymentRequest request)
    {
        switch (request.PaymentScheme)
        {
            case PaymentScheme.Bacs:
                Result = ProcessBacsPayment(account);
                break;

            case PaymentScheme.FasterPayments:
                Result = ProcessFasterPayment(account, request);
                break;

            case PaymentScheme.Chaps:
                Result = ProcessChapsPayment(account);
                break;
        }

        return Result;
    }

    private MakePaymentResult ProcessBacsPayment(Account account)
    {
        Result.Success = account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        
        return Result;
    }

    private MakePaymentResult ProcessFasterPayment(Account account, MakePaymentRequest request)
    {
        if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
        {
            Result.Success = false;
        }
        else if (account.Balance < request.Amount)
        {
            Result.Success = false;
        }

        Result.Success = true;
        return Result;
    }

    private MakePaymentResult ProcessChapsPayment(Account account)
    {
        if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
        {
            Result.Success = false;
        }
        else if (account.Status != AccountStatus.Live)
        {
            Result.Success = false;
        }

        Result.Success = true;
        return Result;
    }

    private void UpdateAccountBalance(Account account, MakePaymentRequest request, MakePaymentResult result, bool isBackupAccount)
    {
        if (!result.Success)
        {
            return;
        }
        if (account.Balance < request.Amount) 
        { 
            result.Success = false;
            return;
        }

        account.Balance -= request.Amount;
        AccountRepository.UpdateAccount(account);
    }
}