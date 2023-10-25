using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;

namespace LinguaContext.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    public ISentenceRepository Sentence { get; private set; }

    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}
