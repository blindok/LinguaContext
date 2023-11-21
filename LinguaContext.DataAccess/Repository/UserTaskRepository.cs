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

    public UserTask CreateUserTask(User user, Sentence sentence)
    {
        return new UserTask()
        {
            UserId      = user.Id,
            SentenceId  = sentence.SentenceId,
            FirstReview = DateTime.Now,
            LastReview  = DateTime.Now,
            NextReview  = DateTime.Now,
            User        = user,
            Sentence    = sentence
        };
    }

    public UserTask? GetNextUserTaskByUserId(int UserId)
    {
        throw new NotImplementedException();
    }

    public bool IsAlreadyLearnt(int userId, int senteceId)
    {
        UserTask? task = GetFirstOrDefault(t => t.UserId == userId && t.SentenceId == senteceId);
        if (task == null) return true;
        return false;
    }
}