using BM.Common.CQRS.Commands.Dispatcher;
using BM.Common.CQRS.Queries.Dispatcher;
using BM.Common.CQRS.Queries;
using BM.Services.BooksManager.BLL.Commands.Books;
using BM.Services.BooksManager.BLL.Queries.Books;
using BM.Services.BooksManager.Core.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BM.Services.BooksManager.BLL.Queries.Authors;
using BM.Services.BooksManager.BLL.Commands.Authors;
using BM.Services.BooksManager.Core.Models.API;

namespace BM.Services.BooksManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public AuthorsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(QueryResultModel<List<AuthorApiModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var query = new GetAuthorsQuery();

            var result = await _queryDispatcher.DispatchAsync<GetAuthorsQuery, QueryResultModel<List<Author>>>(query);
            if (result.HasErrors)
                return BadRequest(result.ErrorMessages);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(QueryResultModel<AuthorApiModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetAuthorQuery(id);

            var result = await _queryDispatcher.DispatchAsync<GetAuthorQuery, QueryResultModel<Author>>(query);
            if (result.HasErrors)
                return BadRequest(result.ErrorMessages);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAuthorCommand command)
        {

            var result = await _commandDispatcher.DispatchAsync(command);
            if (!result.IsValid)
                return BadRequest(result);

            return Ok();

        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateAuthorCommand command)
        {

            var result = await _commandDispatcher.DispatchAsync(command);
            if (!result.IsValid)
                return BadRequest(result);

            return Ok();

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {

            var command = new DeleteAuthorCommand()
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
