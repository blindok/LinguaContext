using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUserTaskRepository : IRepository<UserTask>
{
    UserTask CreateUserTask(int userId, int sentenceId);
    UserTask CreateUserTask(User user, Sentence sentence);
    UserTask? GetNextUserTaskByUserId(int UserId);

    bool IsAlreadyLearnt(int userId, int senteceId);
}
