namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUnitOfWork
{ 
    IUserRepository         Users       { get; }
    ISentenceRepository     Sentences   { get; }
    IUserTaskRepository     UserTasks { get; }

    void Save();
}
