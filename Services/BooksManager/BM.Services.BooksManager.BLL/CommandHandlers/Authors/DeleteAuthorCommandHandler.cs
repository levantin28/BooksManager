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
    public class DeleteAuthorCommandHandler : ICommandHandler<DeleteAuthorCommand>
    {
        private readonly IAuthorsRepository _authorsRepository;
        public DeleteAuthorCommandHandler(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }
        public async Task HandleAsync(DeleteAuthorCommand command)
        {
            await _authorsRepository.DeleteAsync(command.Id);
        }
    }
}
