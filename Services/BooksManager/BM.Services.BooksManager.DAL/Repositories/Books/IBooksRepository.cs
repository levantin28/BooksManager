using BM.Common.Infrastructure.Repositories.Generic;
using BM.Services.BooksManager.Core.Models.Entities;

namespace BM.Services.BooksManager.DAL.Repositories.Books
{
    public interface IBooksRepository : IGenericRepository<Book>
    {
        Task<List<Book>> GetBooksByIds(List<int> ids);
        Task<Book> GetBookByTitle(string title);
        Task<Book> GetBookWithAuthors(int id);
        Task<List<Book>> GetBooksWithAuthors();
    }
}
