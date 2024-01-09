using LinguaContext.Models;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IFolderRepository : IRepository<Folder>
{
    void AddSentenceToFolder(int sentenceId, int folderId);
    void RemoveSentenceFromFolder(int sentenceId, int folderId);
}
