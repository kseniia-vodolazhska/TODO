using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Data.Dtos;
using TodoList.Api.Data.Models;
using TodoList.Api.Data.Repositories;
using TodoList.Api.Framework.Extensions;

namespace TodoList.Api.Controllers {
    [Route("api/todo")]
    //[Authorize]
    [ApiController]
    public class TodoItemController : ControllerBase {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;

        public TodoItemController(ITodoItemRepository todoItemRepository, IMapper mapper) {
            this._todoItemRepository = todoItemRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll() {
            IList<TodoItemDto> response = this._todoItemRepository.GetAll()
                .ProjectTo<TodoItemDto>(this._mapper.ConfigurationProvider)
                .ToList();

            return this.Ok(response);
        }

        [HttpGet]
        [Route("{id}", Name = "GetTodoItem")]
        public IActionResult Get(int id) {
            TodoItem todoItem = this._todoItemRepository.GetById(id);
            if (todoItem == null) {
                return this.NotFound();
            }

            TodoItemDto response = this._mapper.Map<TodoItemDto>(todoItem);
            return this.Ok(response);
        }
        
        [HttpGet]
        [Route("search", Name = "SearchTodoItem")]
        public IActionResult Search([FromQuery] string text) {
            var todoItems = this._todoItemRepository.Search(text);
            
            if (todoItems.Length == 0) {
                return this.NotFound();
            }

            var response = this._mapper.Map<TodoItemDto[]>(todoItems);
            return this.Ok(response);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]TodoItemDto response) {
            TodoItem todoItem = this._mapper.Map<TodoItem>(response);
            todoItem.ApplicationUserId = this.GetUserId();
            todoItem.SortOrder = this._todoItemRepository.GetNextSortOrder();

            this._todoItemRepository.Add(todoItem);
            await this._todoItemRepository.SaveChangesAsync();

            return this.CreatedAtRoute("GetTodoItem", new { id = todoItem.Id }, this._mapper.Map<TodoItemDto>(todoItem));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]TodoItemDto response) {
            TodoItem todoItem = this._todoItemRepository.GetById(id);
            if (todoItem == null) {
                return this.NotFound();
            }

            this._mapper.Map(response, todoItem);
            await this._todoItemRepository.SaveChangesAsync();

            return this.CreatedAtRoute("GetTodoItem", new { id = todoItem.Id }, this._mapper.Map<TodoItemDto>(todoItem));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id) {
            TodoItem todoItem = this._todoItemRepository.GetById(id);
            if (todoItem == null) {
                return this.NotFound();
            }

            this._todoItemRepository.Delete(todoItem);
            await this._todoItemRepository.SaveChangesAsync();

            return this.Ok();
        }
    }
}