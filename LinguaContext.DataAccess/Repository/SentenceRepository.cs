using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguaContext.DataAccess.Repository;

public class SentenceRepository : Repository<Sentence>, ISentenceRepository
{
    internal DbSet<UserSentenceInfo> _dbSetUserSentences { get; set; }

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
            PositionInText = SentencePosition
        };
        var infoFromDb = _dbSetUserSentences.Add(info);
        sentence.UserSentenceInfoId = infoFromDb.Entity.UserSentenceInfoId;
        _dbSet.Add(sentence);
    }
}
