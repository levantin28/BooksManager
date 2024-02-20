using BM.Common.CQRS.Queries;
using BM.Common.CQRS.Queries.Handler;
using BM.Services.BooksManager.BLL.Queries.Authors;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.QueryHandlers.Authors
{
    public class GetAuthorQueryHandler : IQueryHandler<GetAuthorQuery, QueryResultModel<Author>>
    {
        private readonly IAuthorsRepository _authorsRepository;
        public GetAuthorQueryHandler(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }
        public async Task<QueryResultModel<Author>> HandleAsync(GetAuthorQuery query)
        {
            var author = await _authorsRepository.GetAuthorWithBooks(query.Id);
            if (author == null)
                return new QueryResultModel<Author>("No authors were found matching the provided id.");

            return new QueryResultModel<Author>(author);
        }
    }
}
