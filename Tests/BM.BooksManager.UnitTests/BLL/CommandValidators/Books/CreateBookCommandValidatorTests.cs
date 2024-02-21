using BM.Common.CQRS.Commands.Validator;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.BLL.CommandValidators.Authors;
using BM.Services.BooksManager.BLL.CommandValidators.Books;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.BooksManager.UnitTests.BLL.CommandValidators.Books
{
    public class CreateBookCommandValidatorTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private ICommandValidator<CreateBookCommand> _validator;
        public CreateBookCommandValidatorTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _mockBooksRepository = new Mock<IBooksRepository>();
            _validator = new CreateBookCommandValidator(_mockBooksRepository.Object, _mockAuthorsRepository.Object);
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error1()
        {
            var command = new CreateBookCommand()
            {
                Title = "Test",
                Description = "Test",
                AuthorsIds = new List<int> { 1, 2 },
                File = null
            };

            var result = await _validator.ValidateAsync(command);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("Please upload a cover image.", result.ErrorMessages.FirstOrDefault());

        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error2()
        {
            var command = new CreateBookCommand()
            {
                Title = "Test",
                Description = "Test",
                AuthorsIds = new List<int> { 1, 2 },
                File = new FormFile(null, 100, 100, "test", "test.pdf")
            };

            var result = await _validator.ValidateAsync(command);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("Only JPEG, JPG, and PNG files are allowed.", result.ErrorMessages.FirstOrDefault());

        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error3()
        {
            var command = new CreateBookCommand()
            {
                Title = "Test",
                Description = "Test",
                AuthorsIds = new List<int> { 1, 2 },
                File = new FormFile(null, 100, 100, "test", "test.jpg")
            };

            _mockBooksRepository.Setup(x => x.GetBookByTitle(It.IsAny<string>())).ReturnsAsync(new Book()
            {
                Title = "Test"
            });

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetBookByTitle(It.Is<string>(x => x == command.Title)), Times.Once);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("Book already exists in the database.", result.ErrorMessages.FirstOrDefault());

        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error4()
        {
            var command = new CreateBookCommand()
            {
                Title = "Test",
                Description = "Test",
                AuthorsIds = new List<int> { 1, 2 },
                File = new FormFile(null, 100, 100, "test", "test.jpg")
            };
            Book book = null;
            List<Author> authors = null;
            _mockBooksRepository.Setup(x => x.GetBookByTitle(It.IsAny<string>())).ReturnsAsync(book);
            _mockAuthorsRepository.Setup(x => x.GetAuthorsByIds(It.IsAny<List<int>>())).ReturnsAsync(authors);

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetBookByTitle(It.Is<string>(x => x == command.Title)), Times.Once);
            _mockAuthorsRepository.Verify(x => x.GetAuthorsByIds(It.Is<List<int>>(x => x.Contains(command.AuthorsIds.FirstOrDefault()))), Times.Once);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("The authors ids do not match any authors ids in the database.", result.ErrorMessages.FirstOrDefault());

        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Success()
        {
            var command = new CreateBookCommand()
            {
                Title = "Test",
                Description = "Test",
                AuthorsIds = new List<int> { 1, 2 },
                File = new FormFile(null, 100, 100, "test", "test.jpg")
            };
            Book book = null;

            _mockBooksRepository.Setup(x => x.GetBookByTitle(It.IsAny<string>())).ReturnsAsync(book);
            _mockAuthorsRepository.Setup(x => x.GetAuthorsByIds(It.IsAny<List<int>>())).ReturnsAsync(new List<Author>()
            {
                new()
                {
                    Name = "Test",
                    Id = 1
                },
                new()
                {
                    Name = "Test1",
                    Id = 2
                }
            });

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetBookByTitle(It.Is<string>(x => x == command.Title)), Times.Once);
            _mockAuthorsRepository.Verify(x => x.GetAuthorsByIds(It.Is<List<int>>(x => x.Contains(command.AuthorsIds.FirstOrDefault()))), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.IsValid);

        }
    }
}
