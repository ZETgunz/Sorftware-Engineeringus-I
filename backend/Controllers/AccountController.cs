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

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountCredentialsCheck _credentialsCheck;

    public AccountController(IAccountRepository accountRepository, ILogger<AccountController> logger, IAccountCredentialsCheck credentialsCheck)
    {
        _accountRepository = accountRepository;
        _logger = logger;
        _credentialsCheck = credentialsCheck;
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

    [HttpGet("{username}/{password}")]
    public async Task<ActionResult<AccountDTO>> GetAccount([FromRoute] string username, [FromRoute] string password)
    {
        try
        {
            _credentialsCheck.IsUsernameValid(username);
            _credentialsCheck.IsPasswordValid(password);

            var accountDTO = await _accountRepository.GetAccountByUsername(username);
            if (accountDTO == null || accountDTO.Password != password)
            {
                _logger.LogWarning("Account not found with username: {Username} and password: {Password}", username, password);
                return NotFound("Account not found with username: " + username + " and password: " + password);
            }

            return Ok(accountDTO);
        }
        catch (InvalidCredentialsException ex)
        {
            _logger.LogWarning(ex, "Invalid credentials for username: {Username}", username);
            return BadRequest(ex.Message);
        }
        catch (AccountNotFoundException)
        {
            _logger.LogWarning("Account not found with username: {Username}", username);
            return NotFound("Account not found with username: " + username);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting account with username: {Username}", username);
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
            _credentialsCheck.IsUsernameValid(newAccountCreateDTO.Username);
            _credentialsCheck.IsPasswordValid(newAccountCreateDTO.Password);
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
            _credentialsCheck.IsUsernameValid(username);
            _credentialsCheck.IsPasswordValid(updatedAccountUpdateDTO.Password);

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
            _credentialsCheck.IsUsernameValid(username);

            await _accountRepository.DeleteAccount(username);
            return NoContent();
        }
        catch (InvalidCredentialsException ex)
        {
            _logger.LogWarning(ex, "Invalid credentials for deleting account with username: {Username}", username);
            return BadRequest(ex.Message);
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