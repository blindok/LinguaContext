using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUserRepository : IRepository<User>
{
    void Add(User user);

    void CreatePersonalFactorsForUser(int userId, PersonalFactors personalFactors);
    void UpdatePersonalFactors(PersonalFactors personalFactors);
    PersonalFactors? GetPersonalFactorsByUserId(int UserId);
    void DeletePersonalFactors(PersonalFactors personalFactors);
}