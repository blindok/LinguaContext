using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;

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

    public UserTask? GetNextUserTaskByUserId(int UserId)
    {
        throw new NotImplementedException();
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
}