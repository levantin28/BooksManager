using BM.Common.CQRS.Commands;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.Core.Models.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.Commands.Authors
{
    public class UpdateAuthorCommand : BMCommand
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public List<int> BooksIds { get; set; }

        public static Author UpdateAuthor(UpdateAuthorCommand model)
        {
            if (model == null) return null;
            return new Author
            {
                Id = model.Id,
                Name = model.Name,
            };
        }
    }
}
