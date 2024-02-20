using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.DAL.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.CommandHandlers.Books
{
    public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
    {
        private readonly IBooksRepository _booksRepository;
        public DeleteBookCommandHandler(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public async Task HandleAsync(DeleteBookCommand command)
        {
            await _booksRepository.DeleteAsync(command.Id);
        }
    }
}
