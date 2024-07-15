using UrlLoader.Database;
using UrlLoader.Database.Model;

namespace UrlLoader.Producer.Database.Repositories
{
    public class FileEntityRepository : IFileEntityRepository
    {
        private readonly AppDbContext _dbContext;
        public FileEntityRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(FileEntity entity)
        {
            await _dbContext.Files.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
