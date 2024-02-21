using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.CommandHandlers.Authors;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.Core.Models.Entities;
using BM.Services.BooksManager.Core.Models.Relationships;
using BM.Services.BooksManager.DAL.Repositories.Authors;
using BM.Services.BooksManager.DAL.Repositories.BookAuthors;
using BM.Services.BooksManager.DAL.Repositories.Books;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.BooksManager.UnitTests.BLL.CommandHandlers.Authors
{
    public class UpdateAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private readonly Mock<IBookAuthorsRepository> _mockBookAuthorsRepository;

        private ICommandHandler<UpdateAuthorCommand> _handler;
        public UpdateAuthorCommandHandlerTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _mockBooksRepository = new Mock<IBooksRepository>();
            _mockBookAuthorsRepository = new Mock<IBookAuthorsRepository>();
            _handler = new UpdateAuthorCommandHandler(_mockBooksRepository.Object, _mockAuthorsRepository.Object, _mockBookAuthorsRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldUpdateAuthor()
        {
            var command = new UpdateAuthorCommand()
            {
                Id = 99,
                Name = "Test",
                BooksIds = new List<int> { 1, 2 }
            };

            var author = new Author()
            {
                Name = command.Name,
                Id = 99,
                Books = new List<BookAuthor>()
                {
                    new BookAuthor() 
                    { 
                        Book = new Book()
                        {
                            Title = "Test",
                            Id = 1
                        }
                    },
                    new BookAuthor()
                    {
                        Book = new Book()
                        {
                            Title = "Test1",
                            Id = 2
                        }
                    },
                    new BookAuthor()
                    {
                        Book = new Book()
                        {
                            Title = "Test2",
                            Id = 3
                        }
                    }
                }
            };

            _mockAuthorsRepository.Setup(x => x.GetAuthorWithBooks(It.IsAny<int>())).ReturnsAsync(author);
            _mockBooksRepository.Setup(x => x.GetBooksByIds(It.IsAny<List<int>>())).ReturnsAsync(new List<Book>()
            {
                new()
                {
                    Title = "Test",
                    Id = 1
                },
                new()
                {
                    Title = "Test1",
                    Id = 2
                }
            });
            _mockBookAuthorsRepository.Setup(x => x.DeleteAsync(It.IsAny<List<BookAuthor>>()));
            _mockAuthorsRepository.Setup(x => x.AddOrUpdateAsync(It.IsAny<Author>()));

            await _handler.HandleAsync(command);

            _mockAuthorsRepository.Verify(
                  repository => repository.GetAuthorWithBooks(It.Is<int>(a =>
                      a == command.Id
                  )),
                  Times.Once);

            _mockBooksRepository.Verify(
                  repository => repository.GetBooksByIds(It.Is<List<int>>(a =>
                      a.Contains(author.Books.FirstOrDefault().Book.Id)
                  )),
                  Times.Once);

            _mockBookAuthorsRepository.Verify(
                  repository => repository.DeleteAsync(It.Is<List<BookAuthor>>(a => a.Contains(author.Books.FirstOrDefault(x => x.Book.Id == 3)))),
                  Times.Once);

            _mockAuthorsRepository.Verify(
                  repository => repository.AddOrUpdateAsync(It.Is<Author>(a =>
                      a.Name == author.Name
                  )),
                  Times.Once);
        }
    }
}
