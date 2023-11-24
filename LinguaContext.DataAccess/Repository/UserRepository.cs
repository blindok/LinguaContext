using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LinguaContext.DataAccess.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    internal readonly DbSet<PersonalFactors> _dbSetPersonalFactors;
    internal readonly DbSet<PersonalSettings> _dbSetPersonalSettings;

    public UserRepository(ApplicationDbContext db) : base(db) 
    {
        _dbSetPersonalFactors = db.Set<PersonalFactors>();
        _dbSetPersonalSettings = db.Set<PersonalSettings>();
    }

    public void CreatePersonalFactors(int userId, PersonalFactors personalFactors)
    {
        personalFactors.UserId = userId;
        _dbSetPersonalFactors.Add(personalFactors);
    }

    public void UpdatePersonalFactors(PersonalFactors personalFactors)
    {
        _dbSetPersonalFactors.Update(personalFactors);
    }

    public PersonalFactors? GetPersonalFactorsByUserId(int UserId)
    {
        var factors = _dbSetPersonalFactors.FirstOrDefault(x => x.UserId == UserId);
        return factors;
    }

    public void DeletePersonalFactors(PersonalFactors personalFactors) 
    { 
        _dbSetPersonalFactors.Remove(personalFactors);
    }

    public PersonalSettings? GetPersonalSettingsByUserId(int UserId)
    {
        var settings = _dbSetPersonalSettings.FirstOrDefault(x => x.UserId == UserId);
        return settings;
    }

    public void DeletePersonalSettings(PersonalSettings personalSettings)
    {
        _dbSetPersonalSettings.Remove(personalSettings);
    }

    public void UpdatePersonalSettings(PersonalSettings personalSettings)
    {
        _dbSetPersonalSettings.Update(personalSettings);
    }

    public void CreatePersonalSettings(int userId, PersonalSettings personalSettings)
    {
        personalSettings.UserId = userId;
        _dbSetPersonalSettings.Add(personalSettings);
    }
}
