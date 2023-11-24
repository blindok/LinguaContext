using Microsoft.AspNetCore.Mvc;
using LinguaContext.Models.ViewModels;
using LinguaContext.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using LinguaContext.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LinguaContext.DataAccess.Repository.Interfaces;

namespace LinguaContext.Areas.Identity.Controllers;

[Area("Identity")]
public class AuthenticationController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IUnitOfWork unitOfWork, IConfiguration config, ILogger<AuthenticationController> logger)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _logger = logger;
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

        try
        {
            _unitOfWork.Users.Create(registerVM.User);
            _unitOfWork.Save();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return View("Register");
        }

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
        var user = _unitOfWork.Users.GetFirstOrDefault(u => u.Email == loginVM.Email);

        if (user == null) 
        {
            ModelState.AddModelError(String.Empty, "Wrong login or passport!");
            return View();
        }

        if (!BCrypt.Net.BCrypt.Verify(loginVM.Password, user.PasswordHash))
        {
            ModelState.AddModelError(String.Empty, "Wrong login or passport!");
            return View();
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
            new Claim(JwtRegisteredClaimNames.Name, request.UserName),
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
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
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
            UserName = "test",
            UserId = 333,
            Role = SD.Role_Admin,
        };

        return Ok(request);
    }

    //public JsonResult IsUserNameExists(string UserName)
    //{
    //    return Json(_unitOfWork.Users.GetFirstOrDefault(x => x.UserName == UserName), System.Web.Mvc.JsonRequestBehavior.AllowGet);
    //}

    //public JsonResult IsEmailExists(string Email)
    //{
    //    return Json(_unitOfWork.Users.GetFirstOrDefault(x => x.Email == Email), System.Web.Mvc.JsonRequestBehavior.AllowGet);
    //}
}
