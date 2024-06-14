using CosmosDbManagment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDbManagment.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ManagmentContext _context;

        public ProjectController(ManagmentContext managmentContext)
        {
            _context = managmentContext;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var projects = await _context.Projects
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalProjects = await _context.Projects.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling(totalProjects / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(projects);
        }

        [HttpGet("project/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("project/create")]
        public IActionResult Create([FromForm] Project project)
        {
            project.Id = Guid.NewGuid().ToString();
            _context.Projects.Add(project);
            _context.SaveChanges();
            return Redirect("/project");
        }

        [HttpGet("project/edit/{id}")]
        public IActionResult Edit(string id)
        {
            return View(_context.Projects.First(x => x.Id == id));
        }

        [HttpPost("project/edit/{id}")]
        public IActionResult Edit([FromForm] Project project, string id)
        {
            var projectFromDb = _context.Projects.First(x => x.Id == id);
            projectFromDb.Name = project.Name;
            projectFromDb.Description = project.Description;
            projectFromDb.StartDate = project.StartDate;
            projectFromDb.EndDate = project.EndDate;
            _context.Projects.Update(projectFromDb);
            _context.SaveChanges();
            return Redirect("/project");
        }

        [HttpDelete("project/delete/{id}")]
        public IActionResult Delete(string id)
        {
            _context.Remove(_context.Projects.First(x => x.Id == id));
            _context.SaveChanges();
            return Ok();
        }
    }
}
