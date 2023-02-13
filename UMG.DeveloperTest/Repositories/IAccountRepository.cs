using Domain.Entities;

namespace UMG.DeveloperTest.Repositories
{
    public interface IAccountRepository
    {
        public Account GetAccount(string accountNumber);

        public void UpdateAccount(Account account);
    }
}
