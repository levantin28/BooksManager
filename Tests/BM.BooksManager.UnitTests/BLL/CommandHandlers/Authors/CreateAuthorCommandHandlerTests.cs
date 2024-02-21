using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.CommandHandlers.Authors;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.Core.Models.Relationships;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.BooksManager.UnitTests.BLL.CommandHandlers.Authors
{
    public class CreateAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private ICommandHandler<CreateAuthorCommand> _handler;
        public CreateAuthorCommandHandlerTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _handler = new CreateAuthorCommandHandler(_mockAuthorsRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldAddAuthor()
        {
            var command = new CreateAuthorCommand()
            {
                Name = "Test",
            };

            var author = new Author()
            {
                Name = command.Name,
                Id = 1
            };

            _mockAuthorsRepository.Setup(x => x.AddOrUpdateAsync(It.IsAny<Author>()));

            await _handler.HandleAsync(command);

            _mockAuthorsRepository.Verify(
                  repository => repository.AddOrUpdateAsync(It.Is<Author>(a =>
                      a.Name == author.Name 
                  )),
                  Times.Once);
        }
    }
}
