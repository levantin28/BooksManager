using BM.Common.CQRS.Queries;
using BM.Common.CQRS.Queries.Handler;
using BM.Services.BooksManager.BLL.Queries.Books;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.QueryHandlers.Books
{
    public class GetBooksQueryHandler : IQueryHandler<GetBooksQuery, QueryResultModel<List<Book>>>
    {
        private readonly IBooksRepository _booksRepository;
        public GetBooksQueryHandler(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }
        public async Task<QueryResultModel<List<Book>>> HandleAsync(GetBooksQuery query)
        {
            var books = await _booksRepository.GetBooksWithAuthors();
            if (books == null || !books.Any())
                return new QueryResultModel<List<Book>>("No books found.");

            return new QueryResultModel<List<Book>>(books);
        }
    }
}
