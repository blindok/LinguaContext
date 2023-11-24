using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUserRepository : IRepository<User>
{
    void CreatePersonalFactors(int userId, PersonalFactors personalFactors);
    void UpdatePersonalFactors(PersonalFactors personalFactors);
    PersonalFactors? GetPersonalFactorsByUserId(int UserId);
    void DeletePersonalFactors(PersonalFactors personalFactors);

    void CreatePersonalSettings(int userId, PersonalSettings personalSettings);
    PersonalSettings? GetPersonalSettingsByUserId(int UserId);
    void UpdatePersonalSettings(PersonalSettings personalSettings);
    void DeletePersonalSettings(PersonalSettings personalSettings);
}