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
    public class CreateAuthorCommand : BMCommand
    {
        [Required]
        public string Name { get; set; }

        public static Author CreateAuthor(CreateAuthorCommand model)
        {
            if (model == null) return null;
            return new Author
            {
                Name = model.Name,
            };
        }
    }
}
