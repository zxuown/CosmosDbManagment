using CosmosDbManagment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = CosmosDbManagment.Models.Task;

namespace CosmosDbManagment.Controllers
{
    public class TaskController : Controller
    {
        private readonly ManagmentContext _context;
        public TaskController(ManagmentContext managmentContext)
        {
            _context = managmentContext;
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var tasks = await _context.Tasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalTasks = await _context.Tasks.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling(totalTasks / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(tasks);
        }

        [HttpGet("task/create")]
        public IActionResult Create()
        {
            ViewData["projects"] = _context.Projects.ToList();
            return View();
        }

        [HttpPost("task/create")]
        public IActionResult Create([FromForm] Task task)
        {
            task.Id = Guid.NewGuid().ToString();
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return Redirect("/task");
        }

        [HttpGet("task/edit/{id}")]
        public IActionResult Edit(string id)
        {
            ViewData["projects"] = _context.Projects.ToList();
            return View(_context.Tasks.First(x => x.Id == id));
        }

        [HttpPost("task/edit/{id}")]
        public IActionResult Edit([FromForm] Task task, string id)
        {
            var taskFromDb = _context.Tasks.First(x => x.Id == id);
            taskFromDb.Name = task.Name;
            taskFromDb.Description = task.Description;
            taskFromDb.CreatedDate = task.CreatedDate;
            taskFromDb.DueDate = task.DueDate;
            taskFromDb.Status = task.Status;
            taskFromDb.ProjectId = task.ProjectId;
            taskFromDb.AssignedTo = task.AssignedTo;
            _context.Tasks.Update(taskFromDb);
            _context.SaveChanges();
            return Redirect("/task");
        }

        [HttpDelete("task/delete/{id}")]
        public IActionResult Delete(string id)
        {
            _context.Remove(_context.Tasks.First(x => x.Id == id));
            _context.SaveChanges();
            return Ok();
        }
    }
}
