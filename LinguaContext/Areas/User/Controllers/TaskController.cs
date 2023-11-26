using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using LinguaContext.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace LinguaContext.Areas.User.Controlles;

[Area("User")]
[Authorize(Roles = "user,admin")]
public class TaskController : Controller
{
    private readonly IUnitOfWork    _unitOfWork;
    private readonly IMemoryCache   _memoryCache;

    public TaskController(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
    }

    [HttpGet]
    public IActionResult Index(int id)
    {
        PersonalSettings? settings = _unitOfWork.Users.GetPersonalSettingsByUserId(id);
        PersonalStatistics statistics = _unitOfWork.Statistics.GetCurrentStatistics(id);

        _memoryCache.Set("stat" + id.ToString(), statistics);

        if (settings == null)
            settings = new();

        TrainingSettingsVM model = new()
        {
            Settings = settings,
            Statistics = statistics
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(int id, TrainingSettingsVM model)
    {
        _memoryCache.Set("set" + id.ToString(), model.Settings);

        return RedirectToAction(model.TrainingType, new { id = id});
    }

    [HttpGet]
    public IActionResult StandardTraining(int id)
    {
        var settings   = (PersonalSettings?)    _memoryCache.Get("set"  + id.ToString());
        if (settings == null)
        {
            settings = _unitOfWork.Users.GetPersonalSettingsByUserId(id);
            if (settings == null) settings = new();
        }

        Sentence sentence = _unitOfWork.Sentences.GetRandomSentence();

        while (!_unitOfWork.Tasks.IsAlreadyLearnt(id, sentence.SentenceId))
        {
            sentence = _unitOfWork.Sentences.GetRandomSentence();
        }

        UserTask task = _unitOfWork.Tasks.CreateUserTask(id, sentence.SentenceId);
        _memoryCache.Set("task" + id.ToString(), task);

        StandardTrainingVM model = new StandardTrainingVM()
        {
            Sentence = sentence,
            Settings = settings
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult StandardTraining(int id, StandardTrainingVM model)
    {
        UserTask? task = (UserTask?)_memoryCache.Get("task"+id.ToString());
        if (task == null)
        {
            task = _unitOfWork.Tasks.GetUserTask(id, model.Sentence.SentenceId);
        }

        int result = model.ButtonValue;

        task.RepetitionNumber++;
        task.LastReview = DateTime.Now;

        double delay = (task.LastReview - task.NextReview).TotalDays;
        double easeFactor = task.EaseFactor;

        var factors = _unitOfWork.Users.GetPersonalFactorsByUserId(task.UserId);
        if (factors == null)
        {
            factors = new();
        }

        double interval = task.CurrentInterval;

        int interval1 = Convert.ToInt32(Math.Round(interval * factors.FailIntervalModifier));
        if (result == 1)
        {
            task.CurrentInterval = interval1;
            task.EaseFactor = Math.Max(1.3, task.EaseFactor - 0.2);
        }
        else
        {
            int interval2 = Math.Max(
                Convert.ToInt32(Math.Round((interval + delay / 4) * factors.HardIntervalModifier * factors.IntervalModifier)),
                interval1 + 1
            );
            if (result == 2)
            {
                task.CurrentInterval = interval2;
                task.EaseFactor = Math.Max(1.3, task.EaseFactor - 0.15);
            }
            else
            {
                int interval3 = Math.Max(
                    Convert.ToInt32(Math.Round((interval + delay / 2) * easeFactor * factors.IntervalModifier)),
                    interval2 + 1
                    );
                if (result == 3)
                {
                    task.CurrentInterval = interval3;
                }
                else
                {
                    int interval4 = Math.Max(
                        Convert.ToInt32(Math.Round((interval + delay) * easeFactor * factors.IntervalModifier * factors.EasyIntervalModifier)),
                        interval3 + 1
                        );
                    task.CurrentInterval = interval4;
                    task.EaseFactor = Math.Max(1.3, task.EaseFactor + 0.15);
                }
            }
        }

        task.NextReview = task.LastReview.AddDays(task.CurrentInterval);

        if (model.WrongAnswer)
            task.WrongAnswersNumber++;

        if (task.RepetitionNumber == 1)
        {
            _unitOfWork.Tasks.Create(task);
        }
        else
        {
            _unitOfWork.Tasks.Update(task);
        }
        
        _unitOfWork.Save();

        var statistics = (PersonalStatistics?)_memoryCache.Get("stat" + id.ToString());
        if (statistics == null)
        {
            statistics = _unitOfWork.Statistics.GetCurrentStatistics(id);
        }
        statistics.NewBaseTasksNumber++;

        if ((statistics!.NewBaseTasksNumber + statistics!.NewUserTasksNumber) >= model.Settings.NewDailyCardsNumber && true)
            //statistics.ReviewedBaseTasksNumber == statistics.ForReviewBaseTasksNumber)
        {
            return RedirectToAction("FinishTraining", new { id = id});
        }

        return RedirectToAction("StandardTraining", new { id = id});
    }

    [HttpGet]
    public IActionResult FinishTraining(int id)
    {
        PersonalStatistics? statistics = (PersonalStatistics?)_memoryCache.Get("stat" + id.ToString());
        if (statistics != null)
        {
            _unitOfWork.Statistics.Update(statistics);
            _unitOfWork.Save();
        }

        return Ok("You've finished the train!");
    }
}
