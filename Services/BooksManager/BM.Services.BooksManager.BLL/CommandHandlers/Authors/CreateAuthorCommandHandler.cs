using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Services.BooksManager.BLL.CommandHandlers.Authors
{
    public class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand>
    {
        private readonly IAuthorsRepository _authorsRepository;
        public CreateAuthorCommandHandler(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }
        public async Task HandleAsync(CreateAuthorCommand command)
        {
            await _authorsRepository.AddOrUpdateAsync(CreateAuthorCommand.CreateAuthor(command));
        }
    }
}
