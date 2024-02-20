using BM.Common.Infrastructure.Repositories.Generic;
using BM.Services.BooksManager.Core.Models.Entities;

namespace BM.Services.BooksManager.DAL.Repositories.Authors
{
    public interface IAuthorsRepository : IGenericRepository<Author>
    {
        Task<List<Author>> GetAuthorsByIds(List<int> ids);
        Task<Author> GetAuthorByName(string name);
        Task<Author> GetAuthorWithBooks(int id);
        Task<List<Author>> GetAuthorsWithBooks();
    }
}
