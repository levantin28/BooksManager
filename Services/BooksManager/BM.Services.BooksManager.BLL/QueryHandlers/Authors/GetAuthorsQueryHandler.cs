using BM.Common.CQRS.Queries.Handler;
using BM.Common.CQRS.Queries;
using BM.Services.BooksManager.BLL.Queries.Authors;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM.Services.BooksManager.Core.Models.API;

namespace BM.Services.BooksManager.BLL.QueryHandlers.Authors
{
    public class GetAuthorsQueryHandler : IQueryHandler<GetAuthorsQuery, QueryResultModel<List<AuthorApiModel>>>
    {
        private readonly IAuthorsRepository _authorsRepository;
        public GetAuthorsQueryHandler(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }
        public async Task<QueryResultModel<List<AuthorApiModel>>> HandleAsync(GetAuthorsQuery query)
        {
            var authors = await _authorsRepository.GetAuthorsWithBooks();
            if (authors == null || !authors.Any())
                return new QueryResultModel<List<AuthorApiModel>>("No authors found in the database.");

            return new QueryResultModel<List<AuthorApiModel>>(authors);
        }
    }
}
