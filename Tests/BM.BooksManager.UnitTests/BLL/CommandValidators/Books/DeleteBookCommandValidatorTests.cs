using BM.Common.CQRS.Commands.Validator;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.BLL.CommandValidators.Books;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.BooksManager.UnitTests.BLL.CommandValidators.Books
{
    public class DeleteBookCommandValidatorTests
    {
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private ICommandValidator<DeleteBookCommand> _validator;
        public DeleteBookCommandValidatorTests()
        {
            _mockBooksRepository = new Mock<IBooksRepository>();
            _validator = new DeleteBookCommandValidator(_mockBooksRepository.Object);
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error()
        {
            var command = new DeleteBookCommand()
            {
                Id = 99
            };

            Book book = null;

            _mockBooksRepository.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(book);

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetAsync(It.Is<int>(x => x == command.Id), false), Times.Once);
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("The book you are trying to delete doesn't exist.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Success()
        {
            var command = new DeleteBookCommand()
            {
                Id = 99
            };

            _mockBooksRepository.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(new Book()
            {
                Title = "Test",
                Id = 99
            });

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetAsync(It.Is<int>(x => x == command.Id), false), Times.Once);
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            
        }
    }
}
