using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LinguaContext.DataAccess.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    internal DbSet<PersonalFactors> _dbSetPersonalFactors { get; set; }

    public UserRepository(ApplicationDbContext db) : base(db) 
    {
        _dbSetPersonalFactors = db.Set<PersonalFactors>();
    }

    public void Add(User user)
    {
        Create(user);
    }

    public PersonalFactors? GetPersonalFactorsByUserId(int UserId)
    {
        var factors = _dbSetPersonalFactors.FirstOrDefault(x => x.UserId == UserId);
        return factors;
    }

    public void CreatePersonalFactorsForUser(int userId, PersonalFactors personalFactors)
    {
        personalFactors.UserId = userId;
        _dbSetPersonalFactors.Add(personalFactors);
    }

    public void UpdatePersonalFactors(PersonalFactors personalFactors)
    {
        _dbSetPersonalFactors.Update(personalFactors);
    }

    public void DeletePersonalFactors(PersonalFactors personalFactors) 
    { 
        _dbSetPersonalFactors.Remove(personalFactors);
    }
}
