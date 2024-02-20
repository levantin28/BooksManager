using BM.Services.BooksManager.Core.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.Core.Models.API
{
    public class AuthorApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookAuthor> Books { get; set; }
    }
}
