using backend.Models;
using backend.Enums;
using backend.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Mappers;
using backend.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountDTO>>> GetAccounts()
    {
        var accountDTOs = await _accountRepository.GetAllAccounts();
        return Ok(accountDTOs);
    }

    [HttpGet("{username}/{password}")]
    public async Task<ActionResult<AccountDTO>> GetAccount([FromRoute] string username, [FromRoute] string password)
    {
        var accountDTO = await _accountRepository.GetAccountByUsername(username);
        if (accountDTO == null || accountDTO.Password != password)
        {
            return NotFound("Account not found with username: " + username + " and password: " + password);
        }

        return Ok(accountDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AddAccount([FromBody] AccountCreateDTO newAccountCreateDTO)
    {
        if (newAccountCreateDTO == null)
        {
            return BadRequest("Account details cannot be empty.");
        }

        var exists = true;

        var newAccount = newAccountCreateDTO.AccountCreateDTOToAccount();

        try
        {

            var existingAccount = await _accountRepository.GetAccountByUsername(newAccount.Username);
        }
        catch (KeyNotFoundException)
        {
            exists = false;
        }

        if (exists)
        {
            return BadRequest("Account with the same username already exists.");
        }

        await _accountRepository.AddAccount(newAccount.AccountToAccountDTO());
        return CreatedAtAction(nameof(GetAccount), new { username = newAccount.Username, password = newAccount.Password }, newAccount.AccountToAccountDTO());
    }

    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateAccount([FromRoute] string username, [FromBody] AccountUpdateDTO updatedAccountUpdateDTO)
    {
        if (updatedAccountUpdateDTO == null)
        {
            return BadRequest("Account details cannot be empty.");
        }

        var accountDTO = await _accountRepository.GetAccountByUsername(username);
        if (accountDTO == null)
        {
            return NotFound("Account not found with username: " + username);
        }

        accountDTO.Password = updatedAccountUpdateDTO.Password;
        accountDTO.score = updatedAccountUpdateDTO.score;

        await _accountRepository.UpdateAccount(accountDTO);
        return Ok(accountDTO);
    }
}
