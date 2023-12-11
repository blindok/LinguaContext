using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using LinguaContext.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LinguaContext.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "user,admin")]
public class UserSentencesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public UserSentencesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index(int id)
    {
        ViewData["display"] = "base";
        return View();
    }

    [HttpGet]
    public IActionResult CustomSentences(int id)
    {
        ViewData["display"] = "custom";
        return View("Index");
    }

    [HttpPost]
    public IActionResult AddSentence(SentenceVM model)
    {
        int id = int.Parse(User.FindFirst("userid")!.Value);
        Sentence sentence = new()
        {
            Phrase = model.Sentence,
            FormattedPhrase = model.Sentence.Replace(model.Word, "~"),
            Translation = model.Translation,
            Answer = model.Word,
            AnswerTranslation = model.WordTranslation,
            AnswerWordsNumber = 1
        };
        _unitOfWork.Sentences.AddUserSentence(sentence, id, model.Comment);
        _unitOfWork.Save();
        return RedirectToAction("CustomSentences");
    }

    [HttpGet]
    public IActionResult DeleteSentence(int id)
    {
        return RedirectToAction("CustomSentences");
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetTasks()
    {
        int id = int.Parse(User.FindFirst("userid")!.Value);

        var tasksFromDb = _unitOfWork.Tasks.GetAllTasks(id);
        List<UserTask> tasks;
        List<UserSentencesIndexVM> model = new();

        if (tasksFromDb is not null)
        {
            tasks = tasksFromDb.ToList();
            foreach (var task in tasks)
            {
                var sentence = _unitOfWork.Sentences.GetFirstOrDefault(s => s.SentenceId == task.SentenceId)!;

                model.Add(new UserSentencesIndexVM() { Sentence = sentence, Task = task });
            }
        }
        return Json(new { data = model });
    }

    [HttpGet]
    public IActionResult GetUserSentences()
    {
        int id = int.Parse(User.FindFirst("userid")!.Value);
        List<Sentence> sentences = _unitOfWork.Sentences.GetAllUsersSentencesByUserId(id).ToList();

        List<UserSentenceInfoVM> model = new();

        if (sentences is not null)
        {
            foreach (var sentence in sentences)
            {
                var userInfo = _unitOfWork.Sentences.GetUserSentenceInfo(sentence.UserSentenceInfoId);

                model.Add(new() { Sentence = sentence, Info = userInfo });
            }
        }
        for (int i = 0; i < sentences.Count(); ++i)
        {
            model[i].Sentence.UserSentenceInfo = null;
            model[i].Info.Sentence = null;
        }


        return Json(new { data = model });
    }

    #endregion
}
