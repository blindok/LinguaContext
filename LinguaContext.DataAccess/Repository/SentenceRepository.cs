using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LinguaContext.DataAccess.Repository;

public class SentenceRepository : Repository<Sentence>, ISentenceRepository
{
    internal readonly DbSet<UserSentenceInfo> _dbSetUserSentences;

    public SentenceRepository(ApplicationDbContext db) : base(db) 
    {
        _dbSetUserSentences = db.Set<UserSentenceInfo>();
    }

    public IEnumerable<Sentence> GetAllUsersSentences()
    {
        IQueryable<Sentence> sentences = _dbSet.Where(o => o.UserSentenceInfoId != null);
        return sentences;
    }

    public IEnumerable<Sentence> GetAllBuiltInSentences()
    {
        IQueryable<Sentence> sentences = _dbSet.Where(o => o.UserSentenceInfoId == null);
        return sentences;
    }

    public void AddSentence(Sentence sentence)
    {
        _dbSet.Add(sentence);
    }

    public void AddUserSentence(Sentence sentence, int UserId, string? comment = null)
    {
        UserSentenceInfo info = new UserSentenceInfo 
        { 
            UserId=UserId, 
            CreatedAt = DateTime.Now, 
            LastEditedAt = DateTime.Now,
            Comment = comment
        };
        var infoFromDb = _dbSetUserSentences.Add(info);
        _db.SaveChanges();
        sentence.UserSentenceInfoId = infoFromDb.Entity.UserSentenceInfoId;
        _dbSet.Add(sentence);
    }

    public void AddSentenceFromContext(Sentence sentence, int UserId, int ContextId, int SentencePosition, string? comment = null)
    {
        UserSentenceInfo info = new UserSentenceInfo
        {
            UserId = UserId,
            CreatedAt = DateTime.Now,
            LastEditedAt = DateTime.Now,
            Comment = comment,
            SourceContextId = ContextId,
            //PositionInText = SentencePosition
        };
        var infoFromDb = _dbSetUserSentences.Add(info);
        _db.SaveChanges();
        sentence.UserSentenceInfoId = infoFromDb.Entity.UserSentenceInfoId;
        _dbSet.Add(sentence);
    }

    public Sentence? GetRandomSentence()
    {
        return _dbSet.Where(o => o.UserSentenceInfoId == null).OrderBy(r => EF.Functions.Random()).FirstOrDefault();
    }

    public IEnumerable<Sentence> GetAllUsersSentencesByUserId(int userId)
    {
        IQueryable<Sentence> sentences = _dbSet.Where(o => o.UserSentenceInfoId != null);
        
        sentences = sentences.Where(s => s.UserSentenceInfo!.UserId == userId);

        return sentences;
    }

    public UserSentenceInfo? GetUserSentenceInfo(int? id)
    {
        var info = _dbSetUserSentences.FirstOrDefault(i => i.UserSentenceInfoId == id);

        return info;
    }

    public void RemoveUserSentence(int id)
    {
        var sentence = _dbSet.FirstOrDefault(s => s.SentenceId == id);
        if (sentence is not null)
        {
            var info = _dbSetUserSentences.FirstOrDefault(i => i.UserSentenceInfoId == sentence.UserSentenceInfoId);
            _dbSetUserSentences.Remove(info!);
            _dbSet.Remove(sentence);
        }
    }

    public void UpdateUserSentence(Sentence sentence, UserSentenceInfo info)
    {
        _dbSet.Update(sentence);
        _dbSetUserSentences.Update(info);
    }
}
