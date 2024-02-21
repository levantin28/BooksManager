using BM.Common.CQRS.Commands.Handler;
using BM.Common.Infrastructure.Services;
using BM.Services.BooksManager.BLL.CommandHandlers.Authors;
using BM.Services.BooksManager.BLL.CommandHandlers.Books;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.BLL.Commands.Books;
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

namespace BM.BooksManager.UnitTests.BLL.CommandHandlers.Books
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private readonly Mock<IAuthorsRepository> _mockAuthorsRepository;
        private readonly Mock<IFileService> _mockFileService;
        private ICommandHandler<CreateBookCommand> _handler;
        public CreateBookCommandHandlerTests()
        {
            _mockBooksRepository = new Mock<IBooksRepository>();
            _mockAuthorsRepository = new Mock<IAuthorsRepository>();
            _mockFileService = new Mock<IFileService>();
            _handler = new CreateBookCommandHandler(_mockBooksRepository.Object, _mockAuthorsRepository.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldAddBook()
        {
            var command = new CreateBookCommand()
            {
                Title = "Title",
                Description = "Description",
                AuthorsIds = new List<int> { 1, 2 },
                File = new FormFile(null, 100, 100, "Test", "Test")
            };

            var returnedAuthors = new List<Author>()
            {
                new()
                {
                    Name = "Author1",
                    Id = 1
                },
                new()
                {
                    Name = "Author2",
                    Id = 2
                }
            };

            _mockFileService.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>())).ReturnsAsync("test");
            _mockAuthorsRepository.Setup(x => x.GetAuthorsByIds(It.IsAny<List<int>>())).ReturnsAsync(returnedAuthors);
            _mockBooksRepository.Setup(x => x.AddOrUpdateAsync(It.IsAny<Book>()));

            await _handler.HandleAsync(command);

            _mockFileService.Verify(
                  repository => repository.UploadFileAsync(It.Is<IFormFile>(a =>
                      a.FileName == command.File.FileName
                  )),
                  Times.Once);

            _mockAuthorsRepository.Verify(
                  repository => repository.GetAuthorsByIds(It.Is<List<int>>(a =>
                      a.Contains(returnedAuthors.FirstOrDefault().Id)
                  )),
                  Times.Once);

            _mockBooksRepository.Verify(
                  repository => repository.AddOrUpdateAsync(It.Is<Book>(a =>
                      a.Title == command.Title
                  )),
                  Times.Once);
        }

    }
    
}
