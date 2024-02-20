using BM.Common.CQRS.Commands.Handler;
using BM.Common.Infrastructure.Services;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.Core.Models.Relationships;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.CommandHandlers.Books
{
    public class CreateBookCommandHandler : ICommandHandler<CreateBookCommand>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorsRepository _authorsRepository;
        private readonly IFileService _fileService;
        public CreateBookCommandHandler(IBooksRepository booksRepository, IAuthorsRepository authorsRepository, IFileService fileService)
        {
            _authorsRepository = authorsRepository;
            _booksRepository = booksRepository;
            _fileService = fileService;
        }
        public async Task HandleAsync(CreateBookCommand command)
        {
            command.CoverImagePath = await _fileService.UploadFileAsync(command.File);

            var book = CreateBookCommand.CreateBook(command);
            var authors = await _authorsRepository.GetAuthorsByIds(command.AuthorsIds);

            foreach (var author in authors)
            {
                book.Authors.Add(new BookAuthor() { Author = author});
            }

            await _booksRepository.AddOrUpdateAsync(book);

        }
    }
}
