using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.CommandHandlers.Authors;
using BM.Services.BooksManager.BLL.CommandHandlers.Books;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.BooksManager.UnitTests.BLL.CommandHandlers.Books
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private ICommandHandler<DeleteBookCommand> _handler;
        public DeleteBookCommandHandlerTests()
        {
            _mockBooksRepository = new Mock<IBooksRepository>();
            _handler = new DeleteBookCommandHandler(_mockBooksRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldDeleteBook()
        {
            var command = new DeleteBookCommand()
            {
                Id = 99
            };

            _mockBooksRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()));

            await _handler.HandleAsync(command);

            _mockBooksRepository.Verify(
                  repository => repository.DeleteAsync(It.Is<int>(a =>
                      a == command.Id
                  )),
                  Times.Once);
        }
    }
}
