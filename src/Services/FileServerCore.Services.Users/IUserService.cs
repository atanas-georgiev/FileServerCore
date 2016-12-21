namespace FileServerCore.Services.Users
{
    using System.Linq;

    using FileServerCore.Data.Models;

    public interface IUserService
    {
        void Add(User user, string role);

        void Delete(string id);

        IQueryable<User> GetAll();

        User GetById(string id);

        void Update(User user, string role = null);
    }
}