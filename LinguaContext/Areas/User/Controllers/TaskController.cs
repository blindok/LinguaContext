using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using LinguaContext.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        TrainingSettingsVM model = new()
        {
            Settings = new()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(int id, TrainingSettingsVM model)
    {
        return RedirectToAction(model.TrainingType, new { id = id});
    }

    [HttpGet]
    public IActionResult StandardTraining(int id)
    {
        Sentence sentence = _unitOfWork.Sentences.GetRandomSentence();
        while (!_unitOfWork.Tasks.IsAlreadyLearnt(id, sentence.SentenceId))
        {
            sentence = _unitOfWork.Sentences.GetRandomSentence();
        }

        UserTask task = _unitOfWork.Tasks.CreateUserTask(id, sentence.SentenceId);

        int wordsNumber = sentence.Answer.Count(t => t == '~') + 1;

        StandardTrainingVM model = new StandardTrainingVM()
        {
            Sentence = sentence,
            Task = task,
            WordsNumber = wordsNumber,
            Answer = new string[wordsNumber]
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult StandardTraining(StandardTrainingVM model)
    {
        if (model.Answer![0] == model.Sentence.Answer)
        {
            TempData["checkAnswer"] = "You are right!";
        }    
        else
        {
            TempData["checkAnswer"] = "You are wrong!";
        }

        UserTask task = model.Task;
        int result = model.ButtonValue;

        task.RepetitionNumber++;
        task.LastReview = DateTime.UtcNow;

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

        _unitOfWork.Tasks.Create(task);
        _unitOfWork.Save();

        return RedirectToAction("StandardTraining", new { id = model.Task.UserId});
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetRandomSentence()
    {
        Sentence sentence = _unitOfWork.Sentences.GetRandomSentence();
        return Ok(sentence);
    }
}
