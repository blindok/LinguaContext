using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IFavoriteSentencesRepository : IRepository<FavoriteSentence>
{
    void LikeSentence(int UserId, int SentenceId);
    void DisLikeSentence(int UserId, int SentenceId);
    FavoriteSentence? GetFavoriteSentence(int UserId, int SentenceId);

    List<FavoriteSentence> GetAll(int id);
}
