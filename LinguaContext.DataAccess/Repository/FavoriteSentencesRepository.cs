using LinguaContext.DataAccess.Data;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository;

public class FavoriteSentencesRepository : Repository<FavoriteSentence>, IFavoriteSentencesRepository
{
    public FavoriteSentencesRepository(ApplicationDbContext db) : base(db) {}

    public void DisLikeSentence(int UserId, int SentenceId)
    {
        var sentence = _dbSet.FirstOrDefault(s => s.UserId == UserId && s.SentenceId == SentenceId);
        _dbSet.Remove(sentence);
    }

    public List<FavoriteSentence> GetAll(int id)
    {
        return _dbSet.Where(s => s.UserId == id).ToList();
    }

    public FavoriteSentence? GetFavoriteSentence(int UserId, int SentenceId)
    {
        return _dbSet.FirstOrDefault(s => s.UserId == UserId && s.SentenceId == SentenceId);
    }

    public void LikeSentence(int UserId, int SentenceId)
    {
        var sentence = _dbSet.FirstOrDefault(s => s.UserId == UserId && s.SentenceId == SentenceId);

        if (sentence is null)
        {
            sentence = new()
            { 
                UserId = UserId, 
                SentenceId = SentenceId, 
                Date = DateOnly.FromDateTime(DateTime.Now) 
            };

            _dbSet.Add(sentence);
        }  
    }
}
