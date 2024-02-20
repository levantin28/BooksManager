using BM.Common.CQRS.Queries;
using BM.Common.CQRS.Queries.Handler;
using BM.Services.BooksManager.BLL.Queries.Books;
using BM.Services.BooksManager.Core.Models.API;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.QueryHandlers.Books
{
    public class GetBookQueryHandler : IQueryHandler<GetBookQuery, QueryResultModel<Book>> 
    { 
        private readonly IBooksRepository _booksRepository;
        public GetBookQueryHandler(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public async Task<QueryResultModel<Book>> HandleAsync(GetBookQuery query)
        {
            var book = await _booksRepository.GetBookWithAuthors(query.Id);
            if (book == null)
                return new QueryResultModel<Book>("No books were found matching the provided id.");

            return new QueryResultModel<Book>(book);
        }
    }
}
