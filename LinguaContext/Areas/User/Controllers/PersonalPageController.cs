using Microsoft.AspNetCore.Mvc;
using LinguaContext.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using LinguaContext.Models;
using LinguaContext.Utility;

namespace LinguaContext.Areas.User.Controllers;

[Area("User")]
[Authorize]
public class PersonalPageController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    //private static Models.User? CurrentUser { get; set; } = null;

    public PersonalPageController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult Account(int id)
    {
        var user = _unitOfWork.Users.GetFirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            //CurrentUser = null;
            return RedirectToAction("Login", "Authentication", new { Area = "Identity" });
        }

        //CurrentUser = user;

        return View(user);
    }

    [HttpGet]
    public IActionResult Settings(int id)
    {
        return RedirectToAction("UpdatePersonalInfo");
    }

    [HttpGet]
    public IActionResult UpdatePersonalInfo(int id) 
    {
        var user = _unitOfWork.Users.GetFirstOrDefault(x => x.Id == id);
        ViewData["action"] = "PersonalInfo";
        //if (CurrentUser == null) CurrentUser = _unitOfWork.Users.GetFirstOrDefault(x => x.Id == id);
        return View(user);
    }

    [HttpPost]
    public IActionResult UpdatePersonalInfo(Models.User user, IFormFile? file)
    {
        string wwwRootPath = _webHostEnvironment.WebRootPath;

        if (file != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file!.FileName);
            string avatarPath = Path.Combine(wwwRootPath, @"images\avatars");

            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, user.AvatarUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (var fileStream = new FileStream(Path.Combine(avatarPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            user.AvatarUrl = @"\images\avatars\" + fileName;
        }

        _unitOfWork.Users.Update(user);
        _unitOfWork.Save();

        return RedirectToAction("Account", new { id = user.Id });
    }

    [HttpGet]
    public IActionResult EditPersonalFactors(int id)
    {
        //if (CurrentUser == null) CurrentUser = _unitOfWork.Users.GetFirstOrDefault(x => x.Id == id);

        var personalFactors = _unitOfWork.Users.GetPersonalFactorsByUserId(id);

        if (personalFactors == null)
        {
            personalFactors = new();
            personalFactors.UserId = id;
        }

        ViewData["action"] = "PersonalFactors";
        return View(personalFactors);
    }

    [HttpPost]
    public IActionResult EditPersonalFactors(PersonalFactors factors)
    {
        if (factors.IntervalModifier     == DefaultSettings.IntervalModifier     &&
            factors.FailIntervalModifier == DefaultSettings.FailIntervalModifier &&
            factors.HardIntervalModifier == DefaultSettings.HardIntervalModifier &&
            factors.EasyIntervalModifier == DefaultSettings.EasyIntervalModifier )
        {
            if (factors.PersonalFactorsId != 0)
            {
                _unitOfWork.Users.DeletePersonalFactors(factors);
                _unitOfWork.Save();
            }
            return View(factors);
        }

        if (factors.PersonalFactorsId == 0)
        {
            _unitOfWork.Users.CreatePersonalFactorsForUser(factors.UserId, factors);
        }
        else
        {
            _unitOfWork.Users.UpdatePersonalFactors(factors);
        }
        _unitOfWork.Save();

        return View(factors);
    }

    [HttpGet]
    public IActionResult EditPersonalSettings(int id)
    {
        ViewData["action"] = "PersonalSettings";
        return View();
    }
}
