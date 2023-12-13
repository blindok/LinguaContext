using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface ISentenceRepository : IRepository<Sentence>
{
    IEnumerable<Sentence> GetAllUsersSentences();
    IEnumerable<Sentence> GetAllBuiltInSentences();

    void AddSentence(Sentence sentence);
    void AddUserSentence(Sentence sentence, int UserId, string? comment = null);
    void AddSentenceFromContext(Sentence sentence, int UserId, int ContextId, int SentencePosition, string? comment = null);
    void UpdateUserSentence(Sentence sentence, UserSentenceInfo info);
    void RemoveUserSentence(int id);

    Sentence? GetRandomSentence();
    Sentence? GetRandomUserSentence(int userId);

    IEnumerable<Sentence> GetAllUsersSentencesByUserId(int userId);

    UserSentenceInfo? GetUserSentenceInfo(int? sentenceId);

    int CountUserPersonalSentences(int userId);
}
