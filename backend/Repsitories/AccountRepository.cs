using backend.Models;
using backend.DTOs.Account;
using backend.Interfaces;
using backend.Data;
using backend.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccountDTO>> GetAllAccounts()
        {
            return await _context.AccountDTOs.ToListAsync();
        }

        public async Task<AccountDTO> GetAccountByUsername(string username)
        {
            var account = await _context.AccountDTOs.FirstOrDefaultAsync(a => a.Username == username);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found with username: " + username);
            }
            return account;
        }

        public async Task AddAccount(AccountDTO accountDTO)
        {
            var account = accountDTO.AccountDTOToAccount();
            await _context.AccountDTOs.AddAsync(accountDTO);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccount(AccountDTO accountDTO)
        {
            var existingAccount = await _context.AccountDTOs.FirstOrDefaultAsync(a => a.Username == accountDTO.Username);
            if (existingAccount == null)
            {
                throw new KeyNotFoundException("Account not found with username: " + accountDTO.Username);
            }

            existingAccount.Password = accountDTO.Password;
            existingAccount.role = accountDTO.role;
            existingAccount.score = accountDTO.score;

            _context.AccountDTOs.Update(existingAccount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccount(string username)
        {
            var account = await _context.AccountDTOs.FirstOrDefaultAsync(a => a.Username == username);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found with username: " + username);
            }

            _context.AccountDTOs.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}