using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Pages.Menu
{
    public class DetailsModel : PageModel
    {
        private readonly RestaurantWebApp.Data.RestaurantWebAppContext _context;

        public DetailsModel(RestaurantWebApp.Data.RestaurantWebAppContext context)
        {
            _context = context;
        }

      public FoodItem FoodItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FoodItems == null)
            {
                return NotFound();
            }

            var fooditem = await _context.FoodItems.FirstOrDefaultAsync(m => m.ID == id);
            if (fooditem == null)
            {
                return NotFound();
            }
            else 
            {
                FoodItem = fooditem;
            }
            return Page();
        }
    }
}
