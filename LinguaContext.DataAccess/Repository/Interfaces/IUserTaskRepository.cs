﻿using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IUserTaskRepository : IRepository<UserTask>
{
    UserTask CreateUserTask(int userId, int sentenceId);
    UserTask GetUserTask(int userId, int sentenceId);

    int CountUserTasksForReview(int userId);
    int CountBaseTasksForReview(int userId);

    bool IsAlreadyLearnt(int userId, int sentenceId);

    UserTask? GetBaseReviewTask(int userId);
    UserTask? GetUserReviewTask(int userId);

    IEnumerable<UserTask>? GetAllUserTasks(int userId);
    IEnumerable<UserTask>? GetAllBaseTasks(int userId);
    IEnumerable<UserTask>? GetAllTasks(int userId);
}
