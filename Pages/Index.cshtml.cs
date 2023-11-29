using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

	/*	private readonly RestaurantWebAppContext _context;

		public IndexModel(RestaurantWebAppContext context)
		{
			_context = context;
		}

		public IList<FoodItem> FoodItem { get; set; } = default!;
		
		public void OnGet()
		{
			FoodItem = _context.FoodItems.FromSqlRaw("Select * FROM FoodItem").ToList();
		}*/
	}
}