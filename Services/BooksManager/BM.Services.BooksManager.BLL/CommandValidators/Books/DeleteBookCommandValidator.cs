using BM.Common.CQRS.Commands.Validator;
using BM.Common.Models.Validation;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.Books;

namespace BM.Services.BooksManager.BLL.CommandValidators.Books
{
    public class DeleteBookCommandValidator : ICommandValidator<DeleteBookCommand>
    {
        private readonly IBooksRepository _booksRepository;
        public DeleteBookCommandValidator(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public async Task<ValidationResultModel> ValidateAsync(DeleteBookCommand command)
        {
            var book = await _booksRepository.GetAsync(command.Id);
            if (book == null)
                return new ValidationResultModel("The book you are trying to delete doesn't exist.");

            return new ValidationResultModel();
        }
    }
}
