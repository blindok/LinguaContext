namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUnitOfWork
{ 
    ISentenceRepository Sentence { get; }

    void Save();
}
