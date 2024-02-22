using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.Core.Models.Relationships;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.BookAuthors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using Microsoft.VisualBasic;


namespace BM.Services.BooksManager.BLL.CommandHandlers.Books
{
    public class UpdateBookCommandHandler : ICommandHandler<UpdateBookCommand>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorsRepository _authorsRepository;
        private readonly IBookAuthorsRepository _bookAuthorsRepository;
        public UpdateBookCommandHandler(IBooksRepository booksRepository, IAuthorsRepository authorsRepository, IBookAuthorsRepository bookAuthorsRepository)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
            _bookAuthorsRepository = bookAuthorsRepository;
        }
        public async Task HandleAsync(UpdateBookCommand command)
        {
            var book = await _booksRepository.GetBookWithAuthors(command.Id);
            var authors = await _authorsRepository.GetAuthorsByIds(command.AuthorsIds);

            var authorsNotInBookAuthorsIds = authors.Select(a => a.Id).Except(book.Authors.Select(ba => ba.AuthorId));
            var authorsToBeAdded = authors.Where(a => authorsNotInBookAuthorsIds.Contains(a.Id)).ToList();

            var bookAuthorsNotInAuthorsIds = book.Authors.Select(ba => ba.AuthorId).Except(authors.Select(a => a.Id)).ToList();
            var authorsToBeRemoved = book.Authors.Where(ba => bookAuthorsNotInAuthorsIds.Contains(ba.AuthorId)).ToList();

            foreach (var author in authorsToBeAdded)
            {
                if (book.Authors.Any(x => x.Author.Id == author.Id))
                    continue;

                book.Authors.Add(new BookAuthor() { Author = author });
            }

            book.Description = command.Description;

            await _bookAuthorsRepository.DeleteAsync(authorsToBeRemoved);

            await _booksRepository.AddOrUpdateAsync(book);
        }
    }
}
