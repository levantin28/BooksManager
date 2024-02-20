using BM.Common.Infrastructure.Repositories.Generic;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BM.Services.BooksManager.DAL.Repositories.Authors
{
    public class AuthorsRepository : GenericRepository<Author, BMDbContext>, IAuthorsRepository
    {
        public AuthorsRepository(BMDbContext context) : base(context)
        {
        }

        public async Task<List<Author>> GetAuthorsByIds(List<int> ids)
        {
            return await this.GetQuery().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<Author> GetAuthorByName(string name)
        {
            return await this.GetQuery().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Author> GetAuthorWithBooks(int id)
        {
            return this.GetQuery().Include(b => b.Books).ThenInclude(ba => ba.Book)
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<Author>> GetAuthorsWithBooks()
        {
            return this.GetQuery().Include(b => b.Books).ThenInclude(ba => ba.Book).ToList();
        }
    }
}
