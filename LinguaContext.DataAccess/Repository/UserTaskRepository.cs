using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository;

public class UserTaskRepository : Repository<UserTask>, IUserTaskRepository
{
    public UserTaskRepository(ApplicationDbContext db) : base(db) { }

    public void CreateUserTask(int userId, int taskId)
    {
        throw new NotImplementedException();
    }

    public UserTask? GetNextUserTaskByUserId(int UserId)
    {
        throw new NotImplementedException();
    }
}
