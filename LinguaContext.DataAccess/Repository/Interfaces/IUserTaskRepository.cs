using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUserTaskRepository : IRepository<UserTask>
{
    void CreateUserTask(int userId, int taskId);
    UserTask? GetNextUserTaskByUserId(int UserId);
}
