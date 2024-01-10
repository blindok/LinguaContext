using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using LinguaContext.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace LinguaContext.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "user,admin")]
[Route("sentences")]
public class UserSentencesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;

    public UserSentencesController(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
    }

    [HttpGet]
    [Route("tasks")]
    public IActionResult Index(int id)
    {
        ViewData["display"] = "base";
        return View();
    }

    [HttpGet]
    [Route("personal")]
    public IActionResult CustomSentences()
    {
        ViewData["display"] = "custom";
        return View("Index");
    }

    [HttpGet]
    [Route("favorite")]
    public IActionResult FavoriteSentences()
    {
        ViewData["display"] = "fav";
        return View("Index");
    }

    [HttpGet]
    [Route("blacklist")]
    public IActionResult UnwantedSentences()
    {
        ViewData["display"] = "unwanted";
        return View("Index");
    }

    [HttpPost]
    [Route("add")]
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

        var stat = _unitOfWork.Statistics.GetCurrentStatistics(id);
        stat.CreatedTasksNumber += 1;
        _unitOfWork.Statistics.Update(stat);

        _unitOfWork.Save();
        return RedirectToAction("CustomSentences");
    }

    [HttpGet]
    [Route("delete")]
    public IActionResult DeleteSentence(int id)
    {
        _unitOfWork.Sentences.RemoveUserSentence(id);
        _unitOfWork.Save();
        return RedirectToAction("CustomSentences");
    }

    [HttpGet]
    [Route("edit")]
    public IActionResult EditSentence(int id)
    {
        Sentence sentence = _unitOfWork.Sentences.GetFirstOrDefault(s => s.SentenceId == id)!;
        UserSentenceInfo userInfo = _unitOfWork.Sentences.GetUserSentenceInfo(sentence.UserSentenceInfoId)!;

        _memoryCache.Set("sen" + id.ToString(), sentence);
        _memoryCache.Set("inf" + id.ToString(), userInfo);

        SentenceVM model = new()
        {
            Sentence = sentence.Phrase,
            Translation = sentence.Translation,
            Word = sentence.Answer,
            WordTranslation = sentence.AnswerTranslation,
            Comment = userInfo.Comment,
            Id = id
        };
        ViewData["display"] = "custom";
        ViewData["edit"] = "true";
        return View("Index", model);
    }

    [HttpPost]
    [Route("edit")]
    public IActionResult EditSentence(SentenceVM model)
    {
        var sentence = (Sentence)_memoryCache.Get("sen" + model.Id.ToString())!;
        var info = (UserSentenceInfo)_memoryCache.Get("inf" + model.Id.ToString())!;

        sentence.Phrase = model.Sentence;
        sentence.Translation = model.Translation;
        sentence.Answer = model.Word;
        sentence.AnswerTranslation = model.WordTranslation;
        info.Comment = model.Comment;
        info.LastEditedAt = DateTime.Now;

        _unitOfWork.Sentences.UpdateUserSentence(sentence, info);
        _unitOfWork.Save();

        return RedirectToAction("CustomSentences");
    }

    [HttpGet]
    [Route("dislike")]
    public IActionResult DislikeSentence(int id)
    {
        int userId = int.Parse(User.FindFirst("userid")!.Value);
        _unitOfWork.FavoriteSentences.DisLikeSentence(userId, id);
        _unitOfWork.Save();
        return RedirectToAction("FavoriteSentences");
    }

    [HttpGet]
    [Route("redislike")]
    public IActionResult ReDislikeTask(int id)
    {
        int userId = int.Parse(User.FindFirst("userid")!.Value);

        var task = _unitOfWork.Tasks.GetFirstOrDefault(t => t.UserId == userId && t.SentenceId == id);
        task.isUnwanted = false;
        _unitOfWork.Tasks.Update(task);
        _unitOfWork.Save();
        return RedirectToAction("UnwantedSentences");
    }

    #region API CALLS

    [HttpGet]
    [Route("gettasks")]
    public IActionResult GetTasks()
    {
        int id = int.Parse(User.FindFirst("userid")!.Value);

        var alltasksFromDb = _unitOfWork.Tasks.GetAllTasks(id);
        var tasksFromDb = alltasksFromDb.Where(s => s.isUnwanted == false);
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
    [Route("getusertasks")]
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

    [HttpGet]
    [Route("getfavoritesentences")]
    public IActionResult GetFavoriteSentences()
    {
        int id = int.Parse(User.FindFirst("userid")!.Value);

        var favs = _unitOfWork.FavoriteSentences.GetAll(id);

        List<FavoriteSentenceVM> model = new();

        if (favs is not null)
        {
            foreach (var fav in favs)
            {
                var sentence = _unitOfWork.Sentences.GetFirstOrDefault(s => s.SentenceId == fav.SentenceId);

                model.Add(new() { Sentence = sentence!, Date = fav.Date });
            }
        }

        return Json(new { data = model });
    }

    [HttpGet]
    [Route("getunwantedtasks")]
    public IActionResult GetUnwantedTasks()
    {
        int id = int.Parse(User.FindFirst("userid")!.Value);

        var alltasksFromDb = _unitOfWork.Tasks.GetAllTasks(id);
        var tasksFromDb = alltasksFromDb.Where(s => s.isUnwanted == true);
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

    #endregion
}
