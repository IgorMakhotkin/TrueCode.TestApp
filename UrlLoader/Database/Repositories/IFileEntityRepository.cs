using UrlLoader.Database.Model;

namespace UrlLoader.Producer.Database.Repositories
{
    public interface IFileEntityRepository
    {
        Task CreateAsync(FileEntity entity);
    }
}
