using BM.Common.CQRS.Commands;
using BM.Services.BooksManager.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.Commands.Authors
{
    public class DeleteAuthorCommand : BMCommand
    {
        [Required]
        public int Id { get; set; }
    }
}
