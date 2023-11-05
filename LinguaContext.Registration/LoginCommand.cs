using System.Windows.Input;

namespace LinguaContext.Registration;

public class LoginRequest
{ 
    public required string Email { get; set; }
    public required string Password { get; set; }
}