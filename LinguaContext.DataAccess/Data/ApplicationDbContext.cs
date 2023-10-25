using LinguaContext.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguaContext.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Context> Contexts {  get; set; }
    public DbSet<UserSentenceInfo> userSentenceInfos { get; set; }
    public DbSet<Sentence> Sentences { get; set; }
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<SentenceInFolder> SentencesInFolder { get; set; }
    public DbSet<FavoriteSentence> FavoriteSentences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }
}