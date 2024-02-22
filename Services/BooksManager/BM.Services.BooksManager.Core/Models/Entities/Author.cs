using BM.Services.BooksManager.Core.Models.API;
using BM.Services.BooksManager.Core.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.Core.Models.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookAuthor> Books { get; set; }

        public static implicit operator AuthorApiModel(Author model)
        {
            if (model == null) return null;
            return new AuthorApiModel
            {
                Id = model.Id,
                Name = model.Name,
                Books = model.Books,
            };
        }
    }
}
