using BM.Common.CQRS.Commands.Validator;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.BLL.CommandValidators.Authors;
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
    public class UpdateBookCommandValidatorTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private ICommandValidator<UpdateBookCommand> _validator;
        public UpdateBookCommandValidatorTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _mockBooksRepository = new Mock<IBooksRepository>();
            _validator = new UpdateBookCommandValidator(_mockBooksRepository.Object, _mockAuthorsRepository.Object);
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error1()
        {
            var command = new UpdateBookCommand()
            {
                Title = "Test",
                Id = 99
            };

            _mockBooksRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Book>()
            {
                new()
                {
                    Title = "Test1",
                    Id = 1
                }
            });

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("The book you are trying to update doesn't exist.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error2()
        {
            var command = new UpdateBookCommand()
            {
                Title = "Test",
                Id = 99
            };

            _mockBooksRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Book>()
            {
                new()
                {
                    Title = "Test1",
                    Id = 1
                },
                new()
                {
                    Title = "Test2",
                    Id= 99
                }
            });

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("The title can't pe updated.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error3()
        {
            var command = new UpdateBookCommand()
            {
                Title = "Test",
                Id = 99
            };

            _mockBooksRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Book>()
            {
                new()
                {
                    Title = "Test1",
                    Id = 1
                },
                new()
                {
                    Title = "Test",
                    Id= 99
                },
                new()
                {
                    Title = "Test",
                    Id= 2
                }
            });

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("Another book with the same name exists already.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Error4()
        {
            var command = new UpdateBookCommand()
            {
                Title = "Test",
                Id = 99,
                AuthorsIds = new List<int>() { 1,2 }
            };

            List<Author> authors = null;

            _mockBooksRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Book>()
            {
                new()
                {
                    Title = "Test1",
                    Id = 1
                },
                new()
                {
                    Title = "Test",
                    Id= 99
                }
            });
            _mockAuthorsRepository.Setup(x => x.GetAuthorsByIds(It.IsAny<List<int>>())).ReturnsAsync(authors);

            var result = await _validator.ValidateAsync(command);

            _mockBooksRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);
            _mockAuthorsRepository.Verify(x => x.GetAuthorsByIds(It.Is<List<int>>(x => x.Contains(command.AuthorsIds.FirstOrDefault()))));

            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Equal("The authors ids do not match any authors ids in the database.", result.ErrorMessages.FirstOrDefault());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturn_Success()
        {
            var command = new UpdateBookCommand()
            {
                Title = "Test",
                Id = 99,
                AuthorsIds = new List<int>() { 1, 2 }
            };

            _mockBooksRepository.Setup(x => x.GetAsync(null, null, It.IsAny<bool>())).ReturnsAsync(new List<Book>()
            {
                new()
                {
                    Title = "Test1",
                    Id = 1
                },
                new()
                {
                    Title = "Test",
                    Id= 99
                }
            });
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

            _mockBooksRepository.Verify(x => x.GetAsync(null, null, It.Is<bool>(x => x == true)), Times.Once);
            _mockAuthorsRepository.Verify(x => x.GetAuthorsByIds(It.Is<List<int>>(x => x.Contains(command.AuthorsIds.FirstOrDefault()))));

            Assert.NotNull(result);
            Assert.True(result.IsValid);
            
        }
    }
}
