﻿namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUnitOfWork
{ 
    IUserRepository              Users              { get; }
    ISentenceRepository          Sentences          { get; }
    IUserTaskRepository          Tasks              { get; }
    IStatisticsRepository        Statistics         { get; }
    IFavoriteSentencesRepository FavoriteSentences  { get; }

    void Save();
}
