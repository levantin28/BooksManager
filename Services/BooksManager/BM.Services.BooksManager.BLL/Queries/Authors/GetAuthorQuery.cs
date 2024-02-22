using BM.Common.CQRS.Queries;
using BM.Services.BooksManager.Core.Models.API;
using BM.Services.BooksManager.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.Queries.Authors
{
    public class GetAuthorQuery : BMQuery<QueryResultModel<AuthorApiModel>>
    {
        public int Id { get; set; }
        public GetAuthorQuery(int id)
        {

            Id = id;

        }
    }
}
