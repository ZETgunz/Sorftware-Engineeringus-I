using backend.Models;
using backend.DTOs.Account;
using backend.DTOs.Leaderboard;
using System;

namespace backend.Mappers

{
    public static class AccountMapper
    {
        public static AccountDTO AccountToAccountDTO(this Account account)
        {
            return new AccountDTO
            {
                Username = account.Username,
                Password = account.Password,
                role = account.role,
                score = account.score
            };
        }

        public static Account AccountCreateDTOToAccount(this AccountCreateDTO accountCreateDTO)
        {
            return new Account
            (
                accountCreateDTO.Username,
                accountCreateDTO.Password
            );
        }

        public static LeaderBoardAccountDTO AccountToLeaderBoardAccountDTO(this Account account, int rank)
        {
            return new LeaderBoardAccountDTO
            {
                Username = account.Username,
                score = account.score,
                rank = rank
            };
        }
    }
}