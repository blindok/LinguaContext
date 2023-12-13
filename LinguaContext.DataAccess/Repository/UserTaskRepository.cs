using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using System.Text.RegularExpressions;

namespace LinguaContext.DataAccess.Repository;

public class UserTaskRepository : Repository<UserTask>, IUserTaskRepository
{
    public UserTaskRepository(ApplicationDbContext db) : base(db) { }

    public UserTask CreateUserTask(int userId, int sentenceId)
    {
        return new UserTask()
        {
            UserId      = userId,
            SentenceId  = sentenceId,
            FirstReview = DateTime.Now,
            LastReview  = DateTime.Now,
            NextReview  = DateTime.Now,
        };
    } 

    public UserTask GetUserTask(int userId, int sentenceId)
    {
        UserTask? task = GetFirstOrDefault(t => t.UserId == userId && t.SentenceId == sentenceId);
        if (task == null)
        {
            task = CreateUserTask(userId, sentenceId);
        }
        return task;
    }

    public bool IsAlreadyLearnt(int userId, int sentenceId)
    {
        UserTask? task = GetFirstOrDefault(t => t.UserId == userId && t.SentenceId == sentenceId);
        if (task == null) return true;
        return false;
    }

    public int CountBaseTasksForReview(int userId)
    {
        DateTime today = DateTime.Now.Date;
        int n = _dbSet.Where(t => t.UserId == userId && t.NextReview.Date <= today && !t.IsPersonalTask).Count();
        return n;
    }

    public int CountUserTasksForReview(int userId)
    {
        DateTime today = DateTime.Now.Date;
        int n = _dbSet.Where(t => t.UserId == userId && t.NextReview.Date <= today && t.IsPersonalTask).Count();
        return n;
    }

    public UserTask? GetBaseReviewTask(int userId)
    {
        DateTime today = DateTime.Now.Date;
        UserTask? task = _dbSet.Where(t => t.UserId == userId && t.NextReview.Date <= today && !t.IsPersonalTask).FirstOrDefault();
        return task;
    }

    public UserTask? GetUserReviewTask(int userId)
    {
        DateTime today = DateTime.Now.Date;
        UserTask? task = _dbSet.Where(t => t.UserId == userId && t.NextReview.Date <= today && t.IsPersonalTask).FirstOrDefault();
        return task;
    }

    public IEnumerable<UserTask>? GetAllUserTasks(int userId)
    {
        IEnumerable<UserTask>? tasks = _dbSet.Where(t => t.UserId == userId && t.IsPersonalTask);
        if (!tasks.Any())
        {
            tasks = null;
        }
        return tasks;
    }

    public IEnumerable<UserTask>? GetAllBaseTasks(int userId)
    {
        IEnumerable<UserTask>? tasks = _dbSet.Where(t => t.UserId == userId && !t.IsPersonalTask);
        if (!tasks.Any()) 
        { 
            tasks = null; 
        }
        return tasks;
    }

    public IEnumerable<UserTask>? GetAllTasks(int userId)
    {
        IEnumerable<UserTask>? tasks = _dbSet.Where(t => t.UserId == userId);
        if (!tasks.Any())
        {
            tasks = null;
        }
        return tasks;
    }
}