using Microsoft.AspNetCore.Mvc;
using TodoApiWithOpenTelemetry.Models;

namespace TodoApiWithOpenTelemetry.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
	private static readonly List<TodoItem> Todos = new();

	[HttpGet]
	public ActionResult<IEnumerable<TodoItem>> GetAll()
	{
		return Ok(Todos);
	}

	[HttpGet("{id}")]
	public ActionResult<TodoItem> GetById(int id)
	{
		var todo = Todos.FirstOrDefault(t => t.Id == id);
		if (todo == null)
			return NotFound();

		return Ok(todo);
	}

	[HttpPost]
	public ActionResult Create([FromBody] TodoItem todoItem)
	{
		todoItem.Id = Todos.Count > 0 ? Todos.Max(t => t.Id) + 1 : 1;
		Todos.Add(todoItem);
		return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, todoItem);
	}

	[HttpDelete("{id}")]
	public ActionResult Delete(int id)
	{
		var todo = Todos.FirstOrDefault(t => t.Id == id);
		if (todo == null)
			return NotFound();

		Todos.Remove(todo);
		return NoContent();
	}
}
