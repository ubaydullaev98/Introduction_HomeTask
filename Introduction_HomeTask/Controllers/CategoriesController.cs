using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Introduction_HomeTask.Models;

namespace Introduction_HomeTask.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly NorthwindContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(NorthwindContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Categories controller - Index");
            var listOfCategories  = await _context.Categories.ToListAsync();
            return View(listOfCategories);
        }
    }
}
