﻿using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface ISentenceRepository : IRepository<Sentence>
{
    IEnumerable<Sentence> GetAllUsersSentences();
    IEnumerable<Sentence> GetAllBuiltInSentences();

    void AddSentence(Sentence sentence);
    void AddUserSentence(Sentence sentence, int UserId, string? comment = null);
    void AddSentenceFromContext(Sentence sentence, int ContextId, int SentencePosition, string? comment = null);
}