using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.JsonCRUD;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private static readonly JsonCRUD _JsonCRUD = new JsonCRUD("accounts.json");

    private static List<Account> accounts = new List<Account>();

    [HttpPost]
    public IActionResult AddAccount([FromBody] Account newAccount)
    {
        accounts = _JsonCRUD.ReadJsonObject<List<Account>>();
        if (newAccount == null || string.IsNullOrWhiteSpace(newAccount.Username) || string.IsNullOrWhiteSpace(newAccount.Password))
        {
            return BadRequest("Account details cannot be empty.");
        }
        else if (accounts.Any(account => newAccount.Equals(account)))
        {
            return BadRequest("Account with the same username already exists.");
        }

        accounts.Add(new Account(newAccount.Username, newAccount.Password, newAccount.role));
        _JsonCRUD.WriteJsonObject(accounts);
        return Ok(newAccount);
    }
}