using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository;

public class StatisticsRepository : Repository<PersonalStatistics>, IStatisticsRepository
{
    public StatisticsRepository(ApplicationDbContext db) : base(db) { }
}
