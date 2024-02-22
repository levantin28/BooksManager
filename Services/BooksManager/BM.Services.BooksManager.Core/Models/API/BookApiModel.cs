using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.Core.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.Core.Models.API
{
    public class BookApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public ICollection<BookAuthor> Authors { get; set; }

        public static implicit operator Book(BookApiModel model)
        {
            if (model == null) return null;
            return new Book
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                CoverImagePath = model.CoverImage,
                Authors = model.Authors,
            };
        }
    }
}
