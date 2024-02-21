using BM.Common.CQRS.Commands.Validator;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.BLL.CommandValidators.Authors;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.BooksManager.UnitTests.BLL.CommandValidators.Authors
{
    public class DeleteAuthorCommandValidatorTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private ICommandValidator<DeleteAuthorCommand> _validator;
        public DeleteAuthorCommandValidatorTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _validator = new DeleteAuthorCommandValidator(_mockAuthorsRepository.Object);
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error()
        {
            var command = new DeleteAuthorCommand()
            {
               Id = 99,
            };

            Author author = null;

            _mockAuthorsRepository.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(author);

            var result = await _validator.ValidateAsync(command);

            _mockAuthorsRepository.Verify(x => x.GetAsync(It.Is<int>(x => x == command.Id), false), Times.Once);
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("Author doesn't exist in the database.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Success()
        {
            var command = new DeleteAuthorCommand()
            {
                Id = 99,
            };

            _mockAuthorsRepository.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(new Author()
            {
                Name = "Test",
            });

            var result = await _validator.ValidateAsync(command);

            _mockAuthorsRepository.Verify(x => x.GetAsync(It.Is<int>(x => x == command.Id), false), Times.Once);
            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }
    }
}
