using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUserTaskRepository : IRepository<UserTask>
{
    UserTask CreateUserTask(int userId, int sentenceId);
    UserTask GetUserTask(int userId, int sentenceId);

    UserTask? GetNextUserTaskByUserId(int userId);

    bool IsAlreadyLearnt(int userId, int sentenceId);
}
