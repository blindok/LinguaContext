using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IStatisticsRepository : IRepository<PersonalStatistics>
{
    PersonalStatistics GetCurrentStatistics(int userId);
}
