using CosmosDbManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDbManagment.Controllers
{
    public class HomeController : Controller
    {
		private readonly ManagmentContext _context;

        public HomeController(ManagmentContext context)
        {
			_context = context;
			context.Database.EnsureCreated();
		}
		public IActionResult Index()
        {
            return View(_context.Projects.ToList());
        }
    }
}
