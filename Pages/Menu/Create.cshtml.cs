using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantWebApp.Data;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Pages.Menu
{
    [Authorize(Roles = "Admin")]

    public class CreateModel : PageModel
    {
        private readonly RestaurantWebApp.Data.RestaurantWebAppContext _context;

        public CreateModel(RestaurantWebApp.Data.RestaurantWebAppContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FoodItem FoodItem { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

          foreach(var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                FoodItem.ImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();
            }

            _context.FoodItems.Add(FoodItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
