using BM.Common.Infrastructure.Repositories.Generic;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BM.Services.BooksManager.DAL.Repositories.Books
{
    public class BooksRepository : GenericRepository<Book, BMDbContext>, IBooksRepository
    {
        public BooksRepository(BMDbContext context) : base(context)
        {
        }

        public async Task<List<Book>> GetBooksByIds(List<int> ids)
        {
            return await this.GetQuery().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<Book> GetBookByTitle(string title)
        {
            return this.GetQuery().Where(x => x.Title == title).FirstOrDefault();
        }

        public async Task<Book> GetBookWithAuthors(int id)
        {
            return this.GetQuery().Include(b => b.Authors).ThenInclude(ba => ba.Author)
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<Book>> GetBooksWithAuthors()
        {
            return this.GetQuery().Include(b => b.Authors).ThenInclude(ba => ba.Author).ToList();
        }
    }
}
