using BM.Common.CQRS.Commands.Dispatcher;
using BM.Common.CQRS.Queries;
using BM.Common.CQRS.Queries.Dispatcher;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.BLL.Queries.Books;
using BM.Services.BooksManager.Core.Models.API;
using BM.Services.BooksManager.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BM.Services.BooksManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public BooksController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(QueryResultModel<List<BookApiModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var query = new GetBooksQuery();

            var result = await _queryDispatcher.DispatchAsync<GetBooksQuery, QueryResultModel<List<Book>>>(query);
            if (result.HasErrors)
                return BadRequest(result.ErrorMessages);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(QueryResultModel<BookApiModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetBookQuery(id);

            var result = await _queryDispatcher.DispatchAsync<GetBookQuery, QueryResultModel<BookApiModel>>(query);
            if (result.HasErrors)
                return BadRequest(result.ErrorMessages);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateBookCommand command)
        {
            var result = await _commandDispatcher.DispatchAsync(command);
            if (!result.IsValid)
                return BadRequest(result);

            return Ok();

        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateBookCommand command)
        {

            var result = await _commandDispatcher.DispatchAsync(command);
            if (!result.IsValid)
                return BadRequest(result);

            return Ok();

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {

            var command = new DeleteBookCommand()
            {
                Id = id,
            };

            var result = await _commandDispatcher.DispatchAsync(command);
            if (!result.IsValid)
                return BadRequest(result);

            return Ok();

        }
    }
}
