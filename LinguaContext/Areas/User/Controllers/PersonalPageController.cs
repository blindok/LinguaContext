using Microsoft.AspNetCore.Mvc;
using LinguaContext.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using LinguaContext.Models;
using LinguaContext.Utility;

namespace LinguaContext.Areas.User.Controllers;

[Area("User")]
[Authorize]
[Route("user")]
public class PersonalPageController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PersonalPageController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Route("account")]
    public IActionResult Account(int id)
    {
        var user = _unitOfWork.Users.GetFirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            return RedirectToAction("Login", "Authentication", new { Area = "Identity" });
        }

        return View(user);
    }

    [HttpGet]
    [Route("general")]
    public IActionResult Settings(int id)
    {
        return RedirectToAction("UpdatePersonalInfo", new { id = id });
    }

    [HttpGet]
    [Route("info")]
    public IActionResult UpdatePersonalInfo(int id) 
    {
        var user = _unitOfWork.Users.GetFirstOrDefault(x => x.Id == id);
        ViewData["action"] = "PersonalInfo";
        return View(user);
    }

    [HttpPost]
    [Route("info")]
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

        ViewData["action"] = "PersonalInfo";
        return View(user);
    }

    [HttpGet]
    [Route("factors")]
    public IActionResult EditPersonalFactors(int id)
    {
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
    [Route("factors")]
    public IActionResult EditPersonalFactors(int id, PersonalFactors factors)
    {
        ViewData["action"] = "PersonalFactors";
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
            _unitOfWork.Users.CreatePersonalFactors(factors.UserId, factors);
        }
        else
        {
            _unitOfWork.Users.UpdatePersonalFactors(factors);
        }
        _unitOfWork.Save();

        return View(factors);
    }

    [HttpGet]
    [Route("settings")]
    public IActionResult EditPersonalSettings(int id)
    {
        var personalSettings = _unitOfWork.Users.GetPersonalSettingsByUserId(id);

        if (personalSettings == null)
        {
            personalSettings = new();
            personalSettings.UserId = id;
        }

        ViewData["action"] = "PersonalSettings";
        return View(personalSettings);
    }

    [HttpPost]
    [Route("settings")]
    public IActionResult EditPersonalSettings(int id, PersonalSettings settings)
    {
        ViewData["action"] = "PersonalSettings";
        if (settings.NewDailyCardsNumber == DefaultSettings.NewDailyCardsNumber &&
            settings.HighlightAnswer == DefaultSettings.HighlightAnswer &&
            settings.DisplayTranslation == DefaultSettings.DisplayTranslation)
        {
            if (settings.PersonalSettingsId != 0)
            {
                _unitOfWork.Users.DeletePersonalSettings(settings);
                _unitOfWork.Save();
            }
            return RedirectToAction("EditPersonalSettings", new { id = id });
        }

        if (settings.PersonalSettingsId == 0)
        {
            _unitOfWork.Users.CreatePersonalSettings(settings.UserId, settings);
        }
        else
        {
            _unitOfWork.Users.UpdatePersonalSettings(settings);
        }
        _unitOfWork.Save();

        return RedirectToAction("EditPersonalSettings", new { id = id });
    }
}
