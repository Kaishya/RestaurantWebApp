using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Pages
{
	[Authorize (Roles = "Admin, Member")]
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;


		//private readonly RestaurantWebAppContext _db;


		private readonly RestaurantWebAppContext _context;
		
		public IndexModel(RestaurantWebAppContext context, ILogger<IndexModel> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IList<FoodItem> FoodItem { get; set; } = default!;
        
		[BindProperty]
        public string Search { get; set; }
        public void OnGet()
		{
			FoodItem = _context.FoodItems.FromSqlRaw("Select * FROM FoodItem").ToList();
		}

		

		public IActionResult OnPostSearch()
		{
			FoodItem = _context.FoodItems.FromSqlRaw("SELECT * FROM FoodItem WHERE Item_name LIKE'" + Search + "%'").ToList();
			return Page();
		}


	}
}