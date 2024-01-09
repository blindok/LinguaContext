using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using LinguaContext.Models.ViewModels;
using LInguaContext.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        settings ??= new();

        var twoWeeksStatistics = _unitOfWork.Statistics.GetTwoWeeksStatistics(id);

        List<SplineAreaChartData> ChartPoints = new List<SplineAreaChartData>();

        foreach (var stat in twoWeeksStatistics)
        {
            ChartPoints.Add(new() { Period = stat.Date, 
                                    Added = stat.CreatedTasksNumber, 
                                    Started = stat.NewBaseTasksNumber + stat.NewUserTasksNumber,
                                    Reviewed = stat.ReviewedBaseTasksNumber + stat.ReviewedUserTasksNumber
            });
        }
        ViewBag.ChartPoints = ChartPoints;

        TrainingSettingsVM model = new()
        {
            Settings    = settings,
            Statistics  = statistics
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(int id, TrainingSettingsVM model)
    {
        _memoryCache.Set("set" + id.ToString(), model.Settings);
        
        var factors = _unitOfWork.Users.GetPersonalFactorsByUserId(id);
        factors ??= new();
        _memoryCache.Set("fac" + id.ToString(), factors);

        if (model.TrainingType == "CustomTraining")
        {
            _memoryCache.Set("pc" + id.ToString(), _unitOfWork.Sentences.CountUserPersonalSentences(id)
                                                  - _unitOfWork.Tasks.CountUserTasksForReview(id));
        }

        return RedirectToAction(model.TrainingType, new { id = id});
    }

    [HttpGet]
    public IActionResult StandardTraining(int id)
    {

        var settings   = (PersonalSettings?)    _memoryCache.Get("set"  + id.ToString());
        if (settings == null)
        {
            settings = _unitOfWork.Users.GetPersonalSettingsByUserId(id);
            settings ??= new();
        }

        var statistics = (PersonalStatistics?)_memoryCache.Get("stat" + id.ToString());
        statistics ??= _unitOfWork.Statistics.GetCurrentStatistics(id);

        if ((statistics.NewBaseTasksNumber + statistics.NewUserTasksNumber) >= settings.NewDailyCardsNumber &&
            statistics.ReviewedBaseTasksNumber >= statistics.ForReviewBaseTasksNumber)
        {
            return RedirectToAction("FinishTraining", new { id = id });
        }

        int ReviewRemainder = statistics.ForReviewBaseTasksNumber - statistics.ReviewedBaseTasksNumber;
        int NewRemainder = settings.NewDailyCardsNumber - statistics.NewBaseTasksNumber - statistics.NewUserTasksNumber;

        Sentence sentence;
        UserTask? task;
        bool isReviewed = false;

        if (ReviewRemainder < NewRemainder)
        {
            sentence = _unitOfWork.Sentences.GetRandomSentence();
            while (!_unitOfWork.Tasks.IsAlreadyLearnt(id, sentence.SentenceId))
            {
                sentence = _unitOfWork.Sentences.GetRandomSentence();
            }
            task = _unitOfWork.Tasks.CreateUserTask(id, sentence.SentenceId);
        }
        else
        {
            isReviewed = true;
            task = _unitOfWork.Tasks.GetBaseReviewTask(id);
            if (task == null)
            {
                statistics.ReviewedBaseTasksNumber = statistics.ForReviewBaseTasksNumber;
                return RedirectToAction("StandardTraining", new { id = id });
            }
            sentence = _unitOfWork.Sentences.GetFirstOrDefault(s => s.SentenceId == task.SentenceId)!;
        }
        
        _memoryCache.Set("task" + id.ToString(), task);

        var fav = _unitOfWork.FavoriteSentences.GetFavoriteSentence(id, sentence.SentenceId);
        _memoryCache.Set("fav" + id.ToString(), (fav is null) ? false : true);

        StandardTrainingVM model = new StandardTrainingVM()
        {
            Sentence = sentence,
            Statistics = statistics,
            Settings = settings,
            IsReviewed = isReviewed,
            IsLiked = (fav is null) ? false : true,
            IsUnwanted = false
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult StandardTraining(int id, StandardTrainingVM model)
    {
        UserTask? task = (UserTask?)_memoryCache.Get("task"+id.ToString());
        bool wasLiked = (bool?)_memoryCache.Get("fav" + id.ToString()) ?? false;

        task ??= _unitOfWork.Tasks.GetUserTask(id, model.Sentence.SentenceId);

        int result = model.ButtonValue;

        task.RepetitionNumber++;
        task.LastReview = DateTime.Now;
        if (model.WrongAnswer) task.WrongAnswersNumber++;

        double delay = (task.LastReview - task.NextReview).TotalDays;
        double easeFactor = task.EaseFactor;


        var factors = (PersonalFactors?) _memoryCache.Get("fac" + id.ToString());
        if (factors == null)
        {
            factors = _unitOfWork.Users.GetPersonalFactorsByUserId(task.UserId);
            factors ??= new();
        }
 
        double interval = task.CurrentInterval;

        task.CurrentInterval = Interval.ComputeInterval(factors, ref easeFactor, interval, delay, result);

        task.NextReview = task.LastReview.AddDays(task.CurrentInterval);

        task.isUnwanted = model.IsUnwanted;

        if (task.RepetitionNumber == 1)
        {
            _unitOfWork.Tasks.Create(task);
        }
        else
        {
            _unitOfWork.Tasks.Update(task);
        }

        if (model.IsReviewed)
        {
            if (task.CurrentInterval > 1) 
                model.Statistics.ReviewedBaseTasksNumber++;
        }
        else
        {
            model.Statistics.NewBaseTasksNumber++;
            if (task.CurrentInterval <= 1)
                model.Statistics.ForReviewBaseTasksNumber++;
        }

        _memoryCache.Set("set" + id.ToString(), model.Settings);
        _memoryCache.Set("stat" + id.ToString(), model.Statistics);

        _unitOfWork.Statistics.Update(model.Statistics);

        if (model.IsLiked && !wasLiked)
        {
            _unitOfWork.FavoriteSentences.LikeSentence(id, model.Sentence.SentenceId);
        }
        else if (!model.IsLiked && wasLiked)
        {
            _unitOfWork.FavoriteSentences.DisLikeSentence(id, model.Sentence.SentenceId);
        }
 
        _unitOfWork.Save();

        if ((model.Statistics.NewBaseTasksNumber + model.Statistics.NewUserTasksNumber) >= model.Settings.NewDailyCardsNumber && 
            model.Statistics.ReviewedBaseTasksNumber >= model.Statistics.ForReviewBaseTasksNumber)
        {
            return RedirectToAction("FinishTraining", new { id = id});
        }

        return RedirectToAction("StandardTraining", new { id = id});
    }

    [HttpGet]
    public IActionResult CustomTraining(int id)
    {
        var settings = (PersonalSettings?)_memoryCache.Get("set" + id.ToString());
        if (settings == null)
        {
            settings = _unitOfWork.Users.GetPersonalSettingsByUserId(id);
            settings ??= new();
        }

        var statistics = (PersonalStatistics?)_memoryCache.Get("stat" + id.ToString());
        statistics ??= _unitOfWork.Statistics.GetCurrentStatistics(id);

        var newCardsLeft = (int?)_memoryCache.Get("pc" + id.ToString());
        newCardsLeft ??= _unitOfWork.Sentences.CountUserPersonalSentences(id);

        if (((statistics.NewBaseTasksNumber + statistics.NewUserTasksNumber) >= settings.NewDailyCardsNumber || newCardsLeft == 0) &&
            statistics.ReviewedUserTasksNumber >= statistics.ForReviewUserTasksNumber)
        {
            return RedirectToAction("FinishTraining", new { id = id });
        }

        int ReviewRemainder = statistics.ForReviewUserTasksNumber - statistics.ReviewedUserTasksNumber;
        int NewRemainder = settings.NewDailyCardsNumber - statistics.NewBaseTasksNumber - statistics.NewUserTasksNumber;

        Sentence sentence;
        UserTask? task;
        bool isReviewed = false;

        if (ReviewRemainder < NewRemainder)
        {
            sentence = _unitOfWork.Sentences.GetRandomUserSentence(id);
            while (!_unitOfWork.Tasks.IsAlreadyLearnt(id, sentence.SentenceId))
            {
                sentence = _unitOfWork.Sentences.GetRandomUserSentence(id);
            }
            task = _unitOfWork.Tasks.CreateUserTask(id, sentence.SentenceId);
            task.IsPersonalTask = true;
        }
        else
        {
            isReviewed = true;
            task = _unitOfWork.Tasks.GetUserReviewTask(id);
            if (task == null)
            {
                statistics.ReviewedBaseTasksNumber = statistics.ForReviewBaseTasksNumber;
                return RedirectToAction("CustomTraining", new { id = id });
            }
            sentence = _unitOfWork.Sentences.GetFirstOrDefault(s => s.SentenceId == task.SentenceId)!;
        }

        UserSentenceInfo info = _unitOfWork.Sentences.GetUserSentenceInfo(sentence.UserSentenceInfoId)!; 

        _memoryCache.Set("task" + id.ToString(), task);

        CustomTrainingVM model = new()
        {
            Sentence = sentence,
            Statistics = statistics,
            Settings = settings,
            IsReviewed = isReviewed,
            Comment = info.Comment
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult CustomTraining(int id, CustomTrainingVM model)
    {
        UserTask? task = (UserTask?)_memoryCache.Get("task" + id.ToString());
        task ??= _unitOfWork.Tasks.GetUserTask(id, model.Sentence.SentenceId);

        int result = model.ButtonValue;

        task.RepetitionNumber++;
        task.LastReview = DateTime.Now;
        if (model.WrongAnswer) task.WrongAnswersNumber++;

        double delay = (task.LastReview - task.NextReview).TotalDays;
        double easeFactor = task.EaseFactor;

        var factors = (PersonalFactors?)_memoryCache.Get("fac" + id.ToString());
        if (factors == null)
        {
            factors = _unitOfWork.Users.GetPersonalFactorsByUserId(task.UserId);
            factors ??= new();
        }

        double interval = task.CurrentInterval;

        task.CurrentInterval = Interval.ComputeInterval(factors, ref easeFactor, interval, delay, result);

        task.NextReview = task.LastReview.AddDays(task.CurrentInterval);

        if (task.RepetitionNumber == 1)
        {
            _unitOfWork.Tasks.Create(task);
        }
        else
        {
            _unitOfWork.Tasks.Update(task);
        }

        _unitOfWork.Save();

        var newCardsLeft = (int?)_memoryCache.Get("pc" + id.ToString());

        if (model.IsReviewed)
        {
            if (task.CurrentInterval > 1)
                model.Statistics.ReviewedUserTasksNumber++;
        }
        else
        {
            model.Statistics.NewUserTasksNumber++;
            if (task.CurrentInterval <= 1)
                model.Statistics.ForReviewUserTasksNumber++;
            --newCardsLeft;
        }

        _memoryCache.Set("pc" + id.ToString(), newCardsLeft);
        _memoryCache.Set("set" + id.ToString(), model.Settings);
        _memoryCache.Set("stat" + id.ToString(), model.Statistics);

        _unitOfWork.Statistics.Update(model.Statistics);
        _unitOfWork.Save();

        if (((model.Statistics.NewBaseTasksNumber + model.Statistics.NewUserTasksNumber) >= model.Settings.NewDailyCardsNumber || newCardsLeft == 0) &&
            model.Statistics.ReviewedUserTasksNumber >= model.Statistics.ForReviewUserTasksNumber)
        {
            return RedirectToAction("FinishTraining", new { id = id });
        }

        return RedirectToAction("CustomTraining", new { id = id });
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

    public class SplineAreaChartData
    {
        public DateOnly Period;
        public int Added;
        public int Reviewed;
        public int Started;
        public int GER_InflationRate;
    }
}