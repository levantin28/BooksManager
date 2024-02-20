using BM.Common.Infrastructure.Repositories.Generic;
using BM.Services.BooksManager.Core.Models.Relationships;
using BM.Services.BooksManager.DAL.Context;

namespace BM.Services.BooksManager.DAL.Repositories.BookAuthors
{
    public class BookAuthorsRepository : GenericRepository<BookAuthor, BMDbContext>, IBookAuthorsRepository
    {
        public BookAuthorsRepository(BMDbContext context) : base(context)
        {
        }
    }
}
