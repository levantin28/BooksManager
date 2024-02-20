using BM.Common.CQRS.Commands.Validator;
using BM.Common.Models.Validation;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.CommandValidators.Books
{
    public class UpdateBookCommandValidator : ICommandValidator<UpdateBookCommand>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorsRepository _authorsRepository;
        public UpdateBookCommandValidator(IBooksRepository booksRepository, IAuthorsRepository authorsRepository)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
        }
        public async Task<ValidationResultModel> ValidateAsync(UpdateBookCommand command)
        {
            var books = await _booksRepository.GetAsync(asNoTracking: true);
            var book = books.FirstOrDefault(x => x.Id == command.Id);
            if (book == null)
                return new ValidationResultModel("The book you are trying to update doesn't exist.");

            if (book.Title != command.Title)
                return new ValidationResultModel("The title can't pe updated.");

            if (books.FirstOrDefault(x => x.Title == command.Title && x.Id != command.Id) != null)
                return new ValidationResultModel("Another book with the same name exists already.");

            var authors = await _authorsRepository.GetAuthorsByIds(command.AuthorsIds);
            if (authors == null || !authors.Any())
                return new ValidationResultModel("The authors ids do not match any authors ids in the database.");

            return new ValidationResultModel();
        }
    }
}
