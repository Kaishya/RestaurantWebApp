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

namespace RestaurantWebApp.Pages
{
    [Authorize(Roles = "Admin, Member")]
    public class CheckoutModel : PageModel
    {
        private readonly RestaurantWebAppContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public IList<CheckoutItem> Items { get; private set; }
        public OrderHistory Order = new OrderHistory();

        public decimal Total;
        public long AmountPayable;

        public CheckoutModel(RestaurantWebAppContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            Items = _db.CheckoutItems.FromSqlRaw(
                "SELECT FoodItem.ID, FoodItem.Price, " +
                "FoodItem.Item_name, " +
                "BasketItems.BasketID, BasketItems.Quantity " +
                "FROM FoodItem INNER JOIN BasketItems " +
                "On FoodItem.ID = BasketItems.StockID " +
                "WHERE BasketID = {0}", customer.BasketID
                ).ToList();

            foreach (var item in Items)
            {
                Total += (item.Quantity * item.Price);
            }
            AmountPayable = (long)Total;
        }
		/*
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
		*/
		public async Task<IActionResult> OnPostDeleteAsync(int itemId)
		{
			var user = await _userManager.GetUserAsync(User);
			CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

			var itemToDelete = await _db.BasketItems
				.FirstOrDefaultAsync(i => i.StockID == itemId && i.BasketID == customer.BasketID);

			if (itemToDelete != null)
			{
				_db.BasketItems.Remove(itemToDelete);
				await _db.SaveChangesAsync();
			}

			return RedirectToPage("/Checkout");
		}


		[HttpPost]
        public async Task<IActionResult> OnPostUpdateQuantityAsync(int itemId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            var itemToUpdate = _db.BasketItems.FirstOrDefault(i => i.StockID == itemId && i.BasketID == customer.BasketID);

            if (itemToUpdate != null)
            {
                // Update the quantity
                itemToUpdate.Quantity = quantity;

                _db.BasketItems.Update(itemToUpdate);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage("/Checkout");
        }
    }
}
