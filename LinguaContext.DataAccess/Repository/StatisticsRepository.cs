using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguaContext.DataAccess.Repository;

public class StatisticsRepository : Repository<PersonalStatistics>, IStatisticsRepository
{
    internal readonly IUserTaskRepository _tasks;

    public StatisticsRepository(ApplicationDbContext db, IUserTaskRepository tasks) : base(db) 
    { 
        _tasks = tasks;
    }

    public PersonalStatistics GetCurrentStatistics(int userId)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        var lastDayStat = GetFirstOrDefault(s => s.Date == today);

        if (lastDayStat == null)
        {
            lastDayStat = new()
            {
                Date = today,
                UserId = userId
            };
            Create(lastDayStat);
        }
        else
        {
            SetReviewValues(lastDayStat);
        }

        _db.SaveChanges();
        return lastDayStat;
    }

    new public void Create(PersonalStatistics statistics)
    {
        SetReviewValues(statistics);
        base.Create(statistics);
    }

    public void SetReviewValues(PersonalStatistics statistics)
    {
        statistics.ForReviewUserTasksNumber = _tasks.CountUserTasksForReview(statistics.UserId);
        statistics.ForReviewBaseTasksNumber = _tasks.CountBaseTasksForReview(statistics.UserId);
    }
}
