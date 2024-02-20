using BM.Common.CQRS.Commands.Validator;
using BM.Common.Models.Validation;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.CommandValidators.Authors
{
    public class DeleteAuthorCommandValidator : ICommandValidator<DeleteAuthorCommand>
    {
        private readonly IAuthorsRepository _authorsRepository;
        public DeleteAuthorCommandValidator(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }
        public async Task<ValidationResultModel> ValidateAsync(DeleteAuthorCommand command)
        {
            if (_authorsRepository.GetAsync(command.Id) == null)
                return new ValidationResultModel("Author doesn't exist in the database.");

            return new ValidationResultModel();
        }
    }
}
