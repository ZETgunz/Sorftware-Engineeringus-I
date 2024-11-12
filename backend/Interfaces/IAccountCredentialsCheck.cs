using backend.Exceptions;

namespace backend.Interfaces
{
    public interface IAccountCredentialsCheck
    {
        bool IsPasswordValid(string password);
        bool IsUsernameValid(string username);
    }
}