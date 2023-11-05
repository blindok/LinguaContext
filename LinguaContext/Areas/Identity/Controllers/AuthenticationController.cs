using Microsoft.AspNetCore.Mvc;
using LinguaContext.Models.ViewModels;
using LinguaContext.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using LinguaContext.Utility;
using LinguaContext.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using LinguaContext.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace LinguaContext.Areas.Identity.Controllers;

[Area("Identity")]
public class AuthenticationController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;

    public AuthenticationController(ApplicationDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpGet]
    public IActionResult Register()
    {
        RegisterVM registerVM = new RegisterVM()
        {
            User = new Models.User(),
        };
        return View(registerVM);
    }

    [HttpPost]
    public IActionResult Register(RegisterVM registerVM)
    {
        string passpordHash = BCrypt.Net.BCrypt.HashPassword(registerVM.Password);
        registerVM.User.PasswordHash = passpordHash;
        _db.Users.Add(registerVM.User);
        _db.SaveChanges();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginVM loginVM = new LoginVM();
        return View(loginVM);
    }

    [HttpPost]
    public IActionResult Login(LoginVM loginVM)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email ==  loginVM.Email);

        if (user == null) 
        {
            return BadRequest("Wrong login or passport!");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginVM.Password, user.PasswordHash))
        {
            return BadRequest("Wrong login or passport!");
        }

        string token2 = GenerateTokenString(user);

        return RedirectToAction("Index", "Home", new { Area = "User" });
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("token");
        return RedirectToAction("Index", "Home", new { Area="User"});
    }

    [HttpPost]
    public IActionResult GenerateToken([FromBody]TokenGenerationRequest request)
    {
        var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);

        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, request.Email),
            new Claim(JwtRegisteredClaimNames.Email, request.Email),
            new Claim(ClaimTypes.Role, request.Role),
            new Claim("userid", request.UserId.ToString())
        };

        var jwtBearerAuthenticatedClient = new JwtBearerClient
        {
            IsAuthenticated = true,
            AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
            Name = _config["JwtSettings:Issuer"]
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(jwtBearerAuthenticatedClient, claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = _config["JwtSettings:Issuer"],
            Audience = _config["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        CookieOptions co = new CookieOptions
        {
            Expires = DateTime.Now.AddHours(8),
            HttpOnly = true,
            Secure = true
        };
        Response.Cookies.Append("token", jwt, co);

        return Ok(jwt);
    }

    private string GenerateTokenString(Models.User user)
    {
        var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);

        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("userid", user.Id.ToString())
        };

        var jwtBearerAuthenticatedClient = new JwtBearerClient
        {
            IsAuthenticated = true,
            AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
            Name = _config["JwtSettings:Issuer"]
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(jwtBearerAuthenticatedClient, claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = _config["JwtSettings:Issuer"],
            Audience = _config["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        CookieOptions co = new CookieOptions
        {
            Expires = DateTime.Now.AddHours(8),
            HttpOnly = true,
            Secure = true
        };
        Response.Cookies.Append("token", jwt, co);

        return jwt;
    }

    [HttpGet]
    public IActionResult Test()
    {
        var request = new TokenGenerationRequest
        {
            Email = "test@test.com",
            UserId = 333,
            Role = SD.Role_Admin,
        };

        return Ok(request);
    }
}
