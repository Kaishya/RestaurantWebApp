using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Pages.Menu
{
   // [Authorize(Roles = "Member")]

    public class MenuModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RestaurantWebAppContext _db;
        private readonly RestaurantWebApp.Data.RestaurantWebAppContext _context;

        public MenuModel(RestaurantWebAppContext db, UserManager<IdentityUser> userManager, RestaurantWebAppContext context)
        {
            _db = db;
            _userManager = userManager;
            _context = context;
        }



		// public IndexModel(RestaurantWebApp.Data.RestaurantWebAppContext context)
		//{
		//    _context = context;
		// }

		public IList<FoodItem> FoodItem { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_db.FoodItems != null)
            {
                FoodItem = await _db.FoodItems.ToListAsync();
            }
        }

		[BindProperty]
		public string Search { get; set; }

		public IActionResult OnPostSearch()
		{
			string sanitizedSearch = SanitizeSearchInput(Search);

			if (string.IsNullOrWhiteSpace(sanitizedSearch))
			{
				FoodItem = _context.FoodItems.ToList();
			}
			else
			{
				FoodItem = _context.FoodItems
					.FromSqlRaw("SELECT * FROM FoodItem WHERE Item_name LIKE {0}", sanitizedSearch + "%")
					.ToList();
			}

			return Page();
		}


		private string SanitizeSearchInput(string input)
		{
			input = input?.Trim();

			if (string.IsNullOrWhiteSpace(input))
			{
				return string.Empty;
			}

			input = input.Replace("'", "''");

			string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_ ";

			input = new string(input.Where(c => allowedChars.Contains(c)).ToArray());

			int maxLength = 100;
			if (input.Length > maxLength)
			{
				input = input.Substring(0, maxLength);
			}

			return "%" + input + "%";
		}


		public async Task<IActionResult> OnPostBuyAsync(int itemID)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db
                .CheckoutCustomers
                .FindAsync(user.Email);
            var item = _db.BasketItems
                .FromSqlRaw("SELECT * FROM BasketItems WHERE StockID = {0} AND BasketID = {1}", itemID, customer.BasketID)
                .ToList()
                .FirstOrDefault();
            if (item == null)
            {
                BasketItem newItem = new BasketItem
                {
                    BasketID = customer.BasketID,
                    StockID = itemID,
                    Quantity = 1
                };
                _db.BasketItems.Add(newItem);
                await _db.SaveChangesAsync();
            }
            else
            {
                if(item.Quantity < 100)
                {
                item.Quantity = item.Quantity + 1;

                }
                else
                {
                    item.Quantity = 100;

                }

                _db.Attach(item).State = EntityState.Modified;
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Basket not found!", e);
                }
            }
            return RedirectToPage();
        }

    }
}
