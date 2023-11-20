using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinguaContext.Areas.User.Controlles;

[Area("User")]
[Authorize(Roles = "user,admin")]
public class TaskController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult StandardTraining(int id)
    {
        Sentence sentence = _unitOfWork.Sentences.GetRandomSentence();
        return View(sentence);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetRandomSentence()
    {
        Sentence sentence = _unitOfWork.Sentences.GetRandomSentence();
        return Ok(sentence);
    }
}
