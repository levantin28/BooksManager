using BM.Common.CQRS.Commands.Validator;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.BLL.CommandValidators.Authors;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.BooksManager.UnitTests.BLL.CommandValidators.Authors
{
    public class UpdateAuthorCommandValidatorTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private ICommandValidator<UpdateAuthorCommand> _validator;
        public UpdateAuthorCommandValidatorTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _mockBooksRepository = new Mock<IBooksRepository>();
            _validator = new UpdateAuthorCommandValidator(_mockAuthorsRepository.Object, _mockBooksRepository.Object);
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error1()
        {
            var command = new UpdateAuthorCommand()
            {
                Name = "Test",
                Id = 99
            };

            _mockAuthorsRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Author>()
            {
                new()
                {
                    Name = "Test1",
                    Id = 1
                }
            });

            var result = await _validator.ValidateAsync(command);

            _mockAuthorsRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("Author doesn't exist in the database.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error2()
        {
            var command = new UpdateAuthorCommand()
            {
                Name = "Test",
                Id = 99
            };

            _mockAuthorsRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Author>()
            {
                new()
                {
                    Name = "Test1",
                    Id = 1
                },
                new()
                {
                    Name = "Test",
                    Id = 99
                },
                new()
                {
                    Name = "Test",
                    Id = 2
                }
            });

            var result = await _validator.ValidateAsync(command);

            _mockAuthorsRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("Another author already has this name.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error3()
        {
            var command = new UpdateAuthorCommand()
            {
                Name = "Test",
                Id = 99,
                BooksIds = new List<int> { 1, 2 }
            };


            _mockAuthorsRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Author>()
            {
                new()
                {
                    Name = "Test1",
                    Id = 1
                },
                new()
                {
                    Name = "Test",
                    Id = 99
                },
            });

            List<Book> books = null;
            _mockBooksRepository.Setup(x => x.GetBooksByIds(It.IsAny<List<int>>())).ReturnsAsync(books);

            var result = await _validator.ValidateAsync(command);

            _mockAuthorsRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);
            _mockBooksRepository.Verify(x => x.GetBooksByIds(It.Is<List<int>>(x => x.Contains(command.BooksIds.FirstOrDefault()))), Times.Once);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("The books ids do not match any books ids in the database.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Success()
        {
            var command = new UpdateAuthorCommand()
            {
                Name = "Test",
                Id = 99,
                BooksIds = new List<int> { 1, 2 }
            };


            _mockAuthorsRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Author>()
            {
                new()
                {
                    Name = "Test1",
                    Id = 1
                },
                new()
                {
                    Name = "Test",
                    Id = 99
                },
            });

            _mockBooksRepository.Setup(x => x.GetBooksByIds(It.IsAny<List<int>>())).ReturnsAsync(new List<Book>()
            {
                new()
                {
                    Id = 1,
                    Title = "Test1"
                },
                new()
                {
                    Id = 2,
                    Title = "Test2"
                }
            });

            var result = await _validator.ValidateAsync(command);

            _mockAuthorsRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);
            _mockBooksRepository.Verify(x => x.GetBooksByIds(It.Is<List<int>>(x => x.Contains(command.BooksIds.FirstOrDefault()))), Times.Once);

            Assert.NotNull(result);
            Assert.True(result.IsValid);
        }
    }
}
