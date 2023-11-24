using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace LinguaContext.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    public IUserRepository          Users       { get; private set; }
    public ISentenceRepository      Sentences   { get; private set; }
    public IUserTaskRepository      Tasks       { get; private set; }
    public IStatisticsRepository    Statistics  { get; private set; }

    private readonly ApplicationDbContext _db;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ApplicationDbContext db, ILogger<UnitOfWork> logger)
    {
        _db         = db;
        _logger     = logger;
        Users       = new UserRepository(_db);
        Sentences   = new SentenceRepository(_db);
        Tasks       = new UserTaskRepository(_db);
        Statistics  = new StatisticsRepository(_db, Tasks);
    }

    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}
