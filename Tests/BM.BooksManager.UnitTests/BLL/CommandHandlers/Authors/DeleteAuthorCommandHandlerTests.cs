using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.CommandHandlers.Authors;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.BooksManager.UnitTests.BLL.CommandHandlers.Authors
{
    public class DeleteAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private ICommandHandler<DeleteAuthorCommand> _handler;
        public DeleteAuthorCommandHandlerTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _handler = new DeleteAuthorCommandHandler(_mockAuthorsRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldDeleteAuthor()
        {
            var command = new DeleteAuthorCommand()
            {
                Id = 99
            };

            _mockAuthorsRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));

            await _handler.HandleAsync(command);

            _mockAuthorsRepository.Verify(
                  repository => repository.DeleteAsync(It.Is<int>(a =>
                      a == command.Id
                  )),
                  Times.Once);
        }
    }
}
