using BM.Common.CQRS.Queries;
using BM.Services.BooksManager.Core.Models.API;
using BM.Services.BooksManager.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.Queries.Books
{
    public class GetBookQuery : BMQuery<QueryResultModel<Book>>
    {
        public int Id { get; set; }
        public GetBookQuery(int id)
        {

            Id = id;    

        }
    }
}
