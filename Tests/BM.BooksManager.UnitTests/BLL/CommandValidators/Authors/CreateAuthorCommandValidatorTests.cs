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
    public class CreateAuthorCommandValidatorTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private ICommandValidator<CreateAuthorCommand> _validator;
        public CreateAuthorCommandValidatorTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _validator = new CreateAuthorCommandValidator(_mockAuthorsRepository.Object);
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error()
        {
            var command = new CreateAuthorCommand()
            {
                Name = "Test",
            };

            _mockAuthorsRepository.Setup(x => x.GetAuthorByName(It.IsAny<string>())).ReturnsAsync(
                new Author()
                {
                    Name= "Test",
                }
            );

            var result = await _validator.ValidateAsync(command);

            _mockAuthorsRepository.Verify(x => x.GetAuthorByName(It.Is<string>(x => x == command.Name)), Times.Once);
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("The author already exists.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Success()
        {
            var command = new CreateAuthorCommand()
            {
                Name = "Test",
            };

            Author author = null;

            _mockAuthorsRepository.Setup(x => x.GetAuthorByName(It.IsAny<string>())).ReturnsAsync(author);

            var result = await _validator.ValidateAsync(command);

            _mockAuthorsRepository.Verify(x => x.GetAuthorByName(It.Is<string>(x => x == command.Name)), Times.Once);
            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }
    }
}
