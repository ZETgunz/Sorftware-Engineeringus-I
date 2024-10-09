using backend.Models;
using backend.DTOs.Account;
using System;

namespace backend.Mappers

{
    public static class AccountMapper
    {
        public static AccountDTO AccountToAccountDTO(Account account)
        {
            return new AccountDTO
            {
                Username = account.Username,
                Password = account.Password,
                role = account.role,
                score = account.score
            };
        }

        public static Account AccountCreateDTOToAccount(AccountCreateDTO accountCreateDTO)
        {
            return new Account
            (
                accountCreateDTO.Username,
                accountCreateDTO.Password
            );
        }
    }
}