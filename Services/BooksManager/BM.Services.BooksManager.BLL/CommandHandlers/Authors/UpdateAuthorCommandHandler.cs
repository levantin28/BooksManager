using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.Core.Models.Relationships;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.BookAuthors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.CommandHandlers.Authors
{
    public class UpdateAuthorCommandHandler : ICommandHandler<UpdateAuthorCommand>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorsRepository _authorsRepository;
        private readonly IBookAuthorsRepository _bookAuthorsRepository;
        public UpdateAuthorCommandHandler(IBooksRepository booksRepository, IAuthorsRepository authorsRepository, IBookAuthorsRepository bookAuthorsRepository)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
            _bookAuthorsRepository = bookAuthorsRepository;
        }
        public async Task HandleAsync(UpdateAuthorCommand command)
        {
            var author = await _authorsRepository.GetAuthorWithBooks(command.Id);
            var books = await _booksRepository.GetBooksByIds(command.BooksIds);

            var booksNotInAuthorBooksIds = books.Select(a => a.Id).Except(author.Books.Select(ba => ba.BookId));
            var booksToBeAdded = books.Where(a => booksNotInAuthorBooksIds.Contains(a.Id)).ToList();

            var authorBooksNotInBooksIds = author.Books.Select(ba => ba.AuthorId).Except(books.Select(a => a.Id)).ToList();
            var booksToBeRemoved = author.Books.Where(ba => authorBooksNotInBooksIds.Contains(ba.BookId)).ToList();

            foreach (var book in booksToBeAdded)
            {
                if (author.Books.Any(x => x.Book.Id == book.Id))
                    continue;

                author.Books.Add(new BookAuthor() { Book = book });
            }

            await _bookAuthorsRepository.DeleteAsync(booksToBeRemoved);

            await _authorsRepository.AddOrUpdateAsync(author);
        }
    }
}
