using BM.Services.BooksManager.Core.Models.API;
using BM.Services.BooksManager.Core.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.Core.Models.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImagePath { get; set; }
        public ICollection<BookAuthor> Authors { get; set; }

        public static implicit operator BookApiModel(Book model)
        {
            if (model == null) return null;
            return new BookApiModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                CoverImage = model.CoverImagePath,
                Authors = model.Authors,
            };
        }
    }
}
