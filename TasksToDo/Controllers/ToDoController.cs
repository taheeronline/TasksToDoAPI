using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksToDo.DAL;
using TasksToDo.DAL.iServices;
using TasksToDo.DAL.Services;
using TasksToDo.Models;

namespace TasksToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ToDoController : ControllerBase
    {

        private readonly iToDoService _toDoRepo;

        public ToDoController(iToDoService toDoRepo)
        {
            _toDoRepo = toDoRepo;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IEnumerable<ToDoItem>> GetTodoItems()
        {
            return await _toDoRepo.GetAllTasks();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetTodoItem(int id)
        {
            var todoItem = await _toDoRepo.GetTaskByID(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutTodoItem(ToDoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a valid data");
            }

            try
            {
                await _toDoRepo.UpdateTask(todoItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_toDoRepo.ItemExists(todoItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(todoItem);
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ActionName("AddTask")]
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostTodoItem(ToDoItem todoItem)
        {
            await _toDoRepo.AddTask(todoItem);

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _toDoRepo.GetTaskByID(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            await _toDoRepo.DeleteTask(id);

            return NoContent();
        }
       
    }
}
