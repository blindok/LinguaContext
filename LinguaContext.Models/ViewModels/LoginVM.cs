using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinguaContext.Models.ViewModels;

public class LoginVM
{
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
