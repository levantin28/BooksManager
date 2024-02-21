using BM.Common.CQRS.Commands.Handler;
using BM.Services.BooksManager.BLL.CommandHandlers.Authors;
using BM.Services.BooksManager.BLL.CommandHandlers.Books;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.BLL.Commands.Books;
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

namespace BM.BooksManager.UnitTests.BLL.CommandHandlers.Books
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private readonly Mock<IBookAuthorsRepository> _mockBookAuthorsRepository;

        private ICommandHandler<UpdateBookCommand> _handler;
        public UpdateBookCommandHandlerTests()
        {
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _mockBooksRepository = new Mock<IBooksRepository>();
            _mockBookAuthorsRepository = new Mock<IBookAuthorsRepository>();
            _handler = new UpdateBookCommandHandler(_mockBooksRepository.Object, _mockAuthorsRepository.Object, _mockBookAuthorsRepository.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldUpdateBook()
        {
            var command = new UpdateBookCommand()
            {
                Id = 99,
                Title = "Test",
                Description = "Test",
                AuthorsIds = new List<int> { 1, 2 }
            };

            var book = new Book()
            {
                Title = command.Title,
                Id = 99,
                Authors = new List<BookAuthor>()
                {
                    new BookAuthor()
                    {
                        Author = new Author()
                        {
                            Name = "Test",
                            Id = 1
                        }
                    },
                    new BookAuthor()
                    {
                        Author = new Author()
                        {
                            Name = "Test1",
                            Id = 2
                        }
                    },
                    new BookAuthor()
                    {
                        Author = new Author()
                        {
                            Name = "Test2",
                            Id = 3
                        }
                    },
                }
            };

            _mockBooksRepository.Setup(x => x.GetBookWithAuthors(It.IsAny<int>())).ReturnsAsync(book);
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
            _mockBookAuthorsRepository.Setup(x => x.DeleteAsync(It.IsAny<List<BookAuthor>>()));
            _mockBooksRepository.Setup(x => x.AddOrUpdateAsync(It.IsAny<Book>()));

            await _handler.HandleAsync(command);

            _mockBooksRepository.Verify(
                  repository => repository.GetBookWithAuthors(It.Is<int>(a =>
                      a == command.Id
                  )),
                  Times.Once);

            _mockAuthorsRepository.Verify(
                  repository => repository.GetAuthorsByIds(It.Is<List<int>>(a =>
                      a.Contains(book.Authors.FirstOrDefault().Author.Id)
                  )),
                  Times.Once);

            _mockBookAuthorsRepository.Verify(
                  repository => repository.DeleteAsync(It.Is<List<BookAuthor>>(a => a.Contains(book.Authors.FirstOrDefault(x => x.Author.Id == 3)))),
                  Times.Once);

            _mockBooksRepository.Verify(
                  repository => repository.AddOrUpdateAsync(It.Is<Book>(a =>
                      a.Title == book.Title
                  )),
                  Times.Once);
        }
    }
}
