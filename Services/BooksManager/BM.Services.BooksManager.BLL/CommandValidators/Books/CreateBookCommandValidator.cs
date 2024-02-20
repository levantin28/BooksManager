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
    public class CreateBookCommandValidator : ICommandValidator<CreateBookCommand>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorsRepository _authorsRepository;
        public CreateBookCommandValidator(IBooksRepository booksRepository, IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
            _booksRepository = booksRepository;
        }

        public async Task<ValidationResultModel> ValidateAsync(CreateBookCommand command)
        {
            if (command.File == null || command.File.Length == 0)
                return new ValidationResultModel("Please upload a cover image.");
            
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(command.File.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return new ValidationResultModel("Only JPEG, JPG, and PNG files are allowed.");

            var book = await _booksRepository.GetBookByTitle(command.Title);
            if (book != null)
                return new ValidationResultModel("Book already exists in the database.");

            var authors = await _authorsRepository.GetAuthorsByIds(command.AuthorsIds);
            if (authors == null || !authors.Any())
                return new ValidationResultModel("The authors ids do not match any authors ids in the database.");

            return new ValidationResultModel();
        }
    }
}
