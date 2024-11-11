using backend.Models;
using backend.DTOs.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountDTO>> GetAllAccounts();
        Task<AccountDTO> GetAccountByUsername(string username);
        Task AddAccount(AccountDTO account);
        Task UpdateAccount(AccountDTO account);
        Task DeleteAccount(string username);
    }
}