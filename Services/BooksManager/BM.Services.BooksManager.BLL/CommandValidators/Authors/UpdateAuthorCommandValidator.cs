using BM.Common.CQRS.Commands.Validator;
using BM.Common.Models.Validation;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.CommandValidators.Authors
{
    public class UpdateAuthorCommandValidator : ICommandValidator<UpdateAuthorCommand>
    {
        private readonly IAuthorsRepository _authorsRepository;
        private readonly IBooksRepository _booksRepository;
        public UpdateAuthorCommandValidator(IAuthorsRepository authorsRepository, IBooksRepository booksRepository)
        {
            _authorsRepository = authorsRepository;
            _booksRepository = booksRepository;
        }
        public async Task<ValidationResultModel> ValidateAsync(UpdateAuthorCommand command)
        {
            var authors = await _authorsRepository.GetAsync(asNoTracking: true);
            if (authors.FirstOrDefault(x => x.Id == command.Id) == null)
                return new ValidationResultModel("Author doesn't exist in the database.");

            if (authors.FirstOrDefault(x => x.Name == command.Name && x.Id != command.Id) != null)
                return new ValidationResultModel("Another author already has this name.");

            var books = await _booksRepository.GetBooksByIds(command.BooksIds);
            if (books == null || !books.Any())
                return new ValidationResultModel("The books ids do not match any books ids in the database.");

            return new ValidationResultModel();
        }
    }
}
