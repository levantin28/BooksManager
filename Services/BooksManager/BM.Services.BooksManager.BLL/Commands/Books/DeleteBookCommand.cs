using BM.Common.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.Commands.Books
{
    public class DeleteBookCommand : BMCommand
    {
        [Required]
        public int Id { get; set; }
    }
}
