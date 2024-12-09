using backend.Models;
using backend.Enums;
using backend.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Mappers;
using backend.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using backend.Exceptions;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountController> _logger;
        private readonly IValidator<Account> _accountValidator;
        public AccountController(IAccountRepository accountRepository, ILogger<AccountController> logger, IValidator<Account> accountValidator)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _accountValidator = accountValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountDTO>>> GetAccounts()
        {
            try
            {
                var accountDTOs = await _accountRepository.GetAllAccounts();
                return Ok(accountDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting accounts.");
                return BadRequest("An error occurred while processing your request.");
            }
        }
        [HttpPost("getAccount")]
        public async Task<IActionResult> GetAccount([FromBody] AccountLoginDTO accountLoginDTO)
        {
            if (accountLoginDTO == null)
            {
                return BadRequest("Account login details cannot be empty.");
            }

            try
            {
                _accountValidator.Validate(new Account(accountLoginDTO.Username, accountLoginDTO.Password));

                var accountDTO = await _accountRepository.GetAccountByUsername(accountLoginDTO.Username);
                if (accountDTO == null || accountDTO.Password != accountLoginDTO.Password)
                {
                    _logger.LogWarning("Account not found with username: {Username} and password: {Password}", accountLoginDTO.Username, accountLoginDTO.Password);
                    return NotFound("Account not found with username: " + accountLoginDTO.Username);
                }

                return Ok(accountDTO);
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning(ex, "Invalid credentials for account with username: {Username}", accountLoginDTO.Username);
                return BadRequest(ex.Message);
            }
            catch (AccountNotFoundException)
            {
                _logger.LogWarning("Account not found with username: {Username}", accountLoginDTO.Username);
                return NotFound("Account not found with username: " + accountLoginDTO.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting account with username: {Username}", accountLoginDTO.Username);
                return BadRequest("An error occurred while processing your request.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] AccountCreateDTO newAccountCreateDTO)
        {
            if (newAccountCreateDTO == null)
            {
                return BadRequest("Account details cannot be empty.");
            }

            try
            {
                _accountValidator.Validate(new Account(newAccountCreateDTO.Username, newAccountCreateDTO.Password));
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning(ex, "Invalid credentials for new account with username: {Username}", newAccountCreateDTO.Username);
                return BadRequest(ex.Message);
            }

            var exists = true;

            var newAccount = newAccountCreateDTO.AccountCreateDTOToAccount();

            try
            {
                var existingAccount = await _accountRepository.GetAccountByUsername(newAccount.Username);
            }
            catch (AccountNotFoundException)
            {
                exists = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if account exists with username: {Username}", newAccount.Username);
                return BadRequest("An error occurred while processing your request.");
            }

            if (exists)
            {
                _logger.LogWarning("Account with username: {Username} already exists.", newAccount.Username);
                return BadRequest("Account with the same username already exists.");
            }

            try
            {
                await _accountRepository.AddAccount(newAccount.AccountToAccountDTO());
                return CreatedAtAction(nameof(GetAccount), new { username = newAccount.Username, password = newAccount.Password }, newAccount.AccountToAccountDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding account.");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateAccount([FromRoute] string username, [FromBody] AccountUpdateDTO updatedAccountUpdateDTO)
        {
            if (updatedAccountUpdateDTO == null)
            {
                return BadRequest("Account details cannot be empty.");
            }

            try
            {
                if (updatedAccountUpdateDTO.Password != null)
                {
                    _accountValidator.Validate(new Account(username, updatedAccountUpdateDTO.Password));

                    var accountDTO = await _accountRepository.GetAccountByUsername(username);
                    if (accountDTO == null)
                    {
                        _logger.LogWarning("Account not found with username: {Username}", username);
                        return NotFound("Account not found with username: " + username);
                    }

                    accountDTO.Password = updatedAccountUpdateDTO.Password;
                    accountDTO.score = updatedAccountUpdateDTO.score;

                    await _accountRepository.UpdateAccount(accountDTO);
                    return Ok(accountDTO);
                }
                else
                {
                    var accountDTO = await _accountRepository.GetAccountByUsername(username);
                    if (accountDTO == null)
                    {
                        _logger.LogWarning("Account not found with username: {Username}", username);
                        return NotFound("Account not found");
                    }

                    accountDTO.score = updatedAccountUpdateDTO.score;
                    await _accountRepository.UpdateAccount(accountDTO);
                    return Ok(accountDTO);
                }
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning(ex, "Invalid credentials for updating account with username: {Username}", username);
                return BadRequest(ex.Message);
            }
            catch (AccountNotFoundException)
            {
                _logger.LogWarning("Account not found with username: {Username}", username);
                return NotFound("Account not found with username: " + username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating account with username: {Username}", username);
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string username)
        {
            try
            {

                await _accountRepository.DeleteAccount(username);
                return NoContent();
            }
            catch (AccountNotFoundException)
            {
                _logger.LogWarning("Account not found with username: {Username}", username);
                return NotFound("Account not found with username: " + username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting account with username: {Username}", username);
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}