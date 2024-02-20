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
    public class CreateAuthorCommandValidator : ICommandValidator<CreateAuthorCommand>
    {
        private readonly IAuthorsRepository _authorsRepository;
        public CreateAuthorCommandValidator(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }
        public async Task<ValidationResultModel> ValidateAsync(CreateAuthorCommand command)
        {
            if (await _authorsRepository.GetAuthorByName(command.Name) != null)
                return new ValidationResultModel("The author already exists.");

            return new ValidationResultModel();
        }
    }
}
