﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;
using RestaurantWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantWebApp.Pages.Menu
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RestaurantWebAppContext _db;
        private readonly RestaurantWebAppContext _context;

        public IndexModel(RestaurantWebAppContext db, UserManager<IdentityUser> userManager, RestaurantWebAppContext context)
        {
            _db = db;
            _userManager = userManager;
            _context = context;
        }

        public IList<FoodItem> FoodItem { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_db.FoodItems != null)
            {
                FoodItem = await _db.FoodItems.ToListAsync();
            }
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
                item.Quantity = item.Quantity + 1;
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
