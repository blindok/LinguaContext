using LinguaContext.Areas.User.Controlles;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using LinguaContext.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LinguaContext.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class ManagementController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ManagementController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult AddSentences()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddSentences(IFormFile? file)
    {
        if (file == null) { return View(); }

        List<Sentence>? sentencesFromFile;
        using (StreamReader r = new StreamReader(file.OpenReadStream()))
        {
            string json = r.ReadToEnd();
            sentencesFromFile = JsonConvert.DeserializeObject<List<Sentence>>(json);
        }

        if (sentencesFromFile != null)
        {
            foreach (var sentence in sentencesFromFile)
            {
                _unitOfWork.Sentences.AddSentence(sentence);
            }
        }
        _unitOfWork.Save();
        return RedirectToAction("AddSentences");
    }
}
