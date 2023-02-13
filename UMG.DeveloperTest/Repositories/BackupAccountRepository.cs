using Domain.Entities;

namespace UMG.DeveloperTest.Repositories;

public class BackupAccountRepository : IAccountRepository
{
    public Account GetAccount(string accountNumber)
    {
        // Access backup data base to retrieve account, code removed for brevity 
        return new Account();
    }

    public void UpdateAccount(Account account)
    {
        // Update account in backup database, code removed for brevity
    }
}