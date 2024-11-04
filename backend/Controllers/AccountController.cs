using backend.Models;
using backend.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Mappers;
using backend.Data;

[ApiController]
[Route("api/[controller]")]
public class AccountController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    private static List<Account> accounts = new List<Account>();
    private static List<AccountDTO> accountDTOs = new List<AccountDTO>();

    [HttpGet]
    public ActionResult<List<AccountDTO>> GetAccounts()
    {

        accountDTOs = _context.AccountDTOs.ToList();
        return Ok(accountDTOs);
    }


    [HttpGet("{username}")]
    public ActionResult<AccountDTO> GetAccount([FromRoute] string username)
    {
        accountDTOs = _context.AccountDTOs.ToList();
        AccountDTO accountDTO = accountDTOs.Find(accountDTO => accountDTO.Username == username);
        if (accountDTO == null)
        {
            return NotFound("Account not found with username :" + username);
        }
        return Ok(accountDTO);
    }

    [HttpPost]
    public IActionResult AddAccount([FromBody] AccountCreateDTO newAccountCreateDTO)
    {
        if (newAccountCreateDTO == null)
        {
            return BadRequest("Account details cannot be empty.");
        }

        Account newAccount = newAccountCreateDTO.AccountCreateDTOToAccount();

        accountDTOs = _context.AccountDTOs.ToList();
        accounts = accountDTOs.Select(accountDTO => accountDTO.AccountDTOToAccount()).ToList();

        if (string.IsNullOrWhiteSpace(newAccount.Username) || string.IsNullOrWhiteSpace(newAccount.Password))
        {
            return BadRequest("Account details cannot be empty.");
        }
        else if (accounts.Any(account => newAccount.Equals(account)))
        {
            return BadRequest("Account with the same username already exists.");
        }

        _context.AccountDTOs.Add(newAccount.AccountToAccountDTO());
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAccount), new { username = newAccount.Username }, newAccount.AccountToAccountDTO());
    }

    [HttpPut("{username}")]
    public IActionResult UpdateAccount([FromRoute] string username, [FromBody] AccountUpdateDTO updatedAccountUpdateDTO)
    {
        if (updatedAccountUpdateDTO == null)
        {
            return BadRequest("Account details cannot be empty.");
        }

        var accountDTO = _context.AccountDTOs.FirstOrDefault(a => a.Username == username);
        if (accountDTO == null)
        {
            return NotFound("Account not found with username: " + username);
        }

        accountDTO.Password = updatedAccountUpdateDTO.Password;
        accountDTO.score = updatedAccountUpdateDTO.score;

        _context.AccountDTOs.Update(accountDTO);
        _context.SaveChanges();

        return Ok(accountDTO);
    }


}