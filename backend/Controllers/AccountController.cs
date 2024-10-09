using backend.Models;
using backend.DTOs.Account;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using backend.JsonCRUD;
using backend.Mappers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private static readonly JsonCRUD _JsonCRUD = new JsonCRUD("accounts.json");

    private static List<Account> accounts = new List<Account>();

    [HttpGet]
    public ActionResult<IEnumerable<AccountDTO>> GetAccounts()
    {
        accounts = _JsonCRUD.ReadJsonObject<List<Account>>();
        return Ok(accounts.Select(account => AccountMapper.AccountToAccountDTO(account)));
    }


    [HttpGet("{username}")]
    public ActionResult<AccountDTO> GetAccount([FromRoute] string username)
    {
        accounts = _JsonCRUD.ReadJsonObject<List<Account>>();
        Account account = accounts.Find(account => account.Username == username);
        if (account == null)
        {
            return NotFound("Account not found with username :" + username);
        }
        return Ok(AccountMapper.AccountToAccountDTO(account));
    }

    [HttpPost]
    public IActionResult AddAccount([FromBody] AccountCreateDTO newAccountCreateDTO)
    {
        if (newAccountCreateDTO == null)
        {
            return BadRequest("Account details cannot be empty.");
        }

        Account newAccount = AccountMapper.AccountCreateDTOToAccount(newAccountCreateDTO);

        accounts = _JsonCRUD.ReadJsonObject<List<Account>>();
        if (string.IsNullOrWhiteSpace(newAccount.Username) || string.IsNullOrWhiteSpace(newAccount.Password))
        {
            return BadRequest("Account details cannot be empty.");
        }
        else if (accounts.Any(account => newAccount.Equals(account)))
        {
            return BadRequest("Account with the same username already exists.");
        }

        accounts.Add(newAccount);
        _JsonCRUD.WriteJsonObject(accounts);
        return CreatedAtAction(nameof(GetAccount), new { username = newAccount.Username }, AccountMapper.AccountToAccountDTO(newAccount));
    }


}