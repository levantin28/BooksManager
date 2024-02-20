using BM.Common.CQRS.Commands;
using BM.Services.BooksManager.Core.Models.API;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.Core.Models.Relationships;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.Commands.Books
{
    public class CreateBookCommand : BMCommand
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public string CoverImagePath { get; set; }
        [Required]
        public List<int> AuthorsIds { get; set; }
        [Required]
        public IFormFile File { get; set; } 

        public static Book CreateBook(CreateBookCommand model)
        {
            if (model == null) return null;
            return new Book
            {
                Title = model.Title,
                Description = model.Description,
                CoverImagePath = model.CoverImagePath,
                Authors = new List<BookAuthor>()
            };
        }
    }
}
