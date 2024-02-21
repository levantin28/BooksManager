using BM.Common.CQRS.Commands;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.Core.Models.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.Commands.Books
{
    public class UpdateBookCommand : BMCommand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public List<int> AuthorsIds { get; set; }

        public static Book UpdateBook(UpdateBookCommand model)
        {
            if (model == null) return null;
            return new Book
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Authors = new List<BookAuthor>()
            };
        }
    }
}
