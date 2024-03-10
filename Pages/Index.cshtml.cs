using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantWebApp.Data;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RestaurantWebAppContext _context;

        public IndexModel(RestaurantWebAppContext context, ILogger<IndexModel> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IList<FoodItem> FoodItem { get; set; } = default!;

        [BindProperty]
        public string Search { get; set; }

        public void OnGet()
        {
            FoodItem = _context.FoodItems.FromSqlRaw("SELECT * FROM FoodItem").ToList();
        }

        public IActionResult OnPostSearch()
        {
            FoodItem = _context.FoodItems.FromSqlRaw($"SELECT * FROM FoodItem WHERE Item_name LIKE '{Search}%'").ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostBuyAsync(int itemID)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _context.CheckoutCustomers.FindAsync(user.Email);
            var item = _context.BasketItems
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
                _context.BasketItems.Add(newItem);
            }
            else
            {
                if (item.Quantity < 100)
                {
                    item.Quantity = item.Quantity + 1;

                }
                else
                {
                    item.Quantity = 100;

                }
                _context.Attach(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}
