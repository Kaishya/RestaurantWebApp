using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RestaurantWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantWebApp.Models;
using Stripe;

namespace RestaurantWebApp.Pages
{
    public class SuccessModel : PageModel
    {
        private readonly RestaurantWebAppContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public IList<CheckoutItem> Items { get; private set; }
        public OrderHistory Order = new OrderHistory();

        public decimal Total;
        public long AmountPayable;

        public SuccessModel(RestaurantWebAppContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            await OnPostBuyAsync(); // Call the OnPostBuyAsync method when navigating to the Success page
        }

        public async Task<IActionResult> OnPostBuyAsync()
        {
            var currentOrder = _db.OrderHistories.FromSqlRaw("SELECT * From OrderHistories")
                .OrderByDescending(b => b.OrderNo)
                .FirstOrDefault();

            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }

            var user = await _userManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);
            var basketItems = _db.BasketItems.FromSqlRaw("SELECT * From BasketItems WHERE BasketID = {0}", customer.BasketID).ToList();

            foreach (var item in basketItems)
            {
                RestaurantWebApp.Data.OrderItem oi = new RestaurantWebApp.Data.OrderItem
                {
                    OrderNo = Order.OrderNo,
                    StockID = item.StockID,
                    Quantity = item.Quantity
                };
                _db.OrderItems.Add(oi);
                _db.BasketItems.Remove(item);
            }

            await _db.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
