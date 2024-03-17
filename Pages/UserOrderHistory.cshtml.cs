using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Pages
{
    public class UserOrderHistoryModel : PageModel
    {
        private readonly RestaurantWebAppContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public IList<OrderHistory> OrderHistory { get; set; }
        public IDictionary<int, List<(string ItemName, int Quantity)>> OrderItems { get; set; }

        public UserOrderHistoryModel(RestaurantWebAppContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            OrderHistory = await _db.OrderHistories.Where(o => o.Email == user.Email).ToListAsync();
            OrderItems = new Dictionary<int, List<(string ItemName, int Quantity)>>();

            foreach (var order in OrderHistory)
            {
                var items = await _db.OrderItems
                    .Where(o => o.OrderNo == order.OrderNo && o.Quantity > 0)
                    .Select(o => new { o.StockID, o.Quantity })
                    .ToListAsync();

                if (items.Any())
                {
                    var itemNamesAndQuantities = new List<(string ItemName, int Quantity)>();
                    foreach (var item in items)
                    {
                        var itemName = _db.FoodItems
                            .Where(f => f.ID == item.StockID)
                            .Select(f => f.Item_name)
                            .FirstOrDefault();

                        itemNamesAndQuantities.Add((itemName, item.Quantity));
                    }

                    OrderItems.Add(order.OrderNo, itemNamesAndQuantities);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddOrderToCartAsync(int orderNo)
        {
            var user = await _userManager.GetUserAsync(User);
            var customer = await _db.CheckoutCustomers.FirstOrDefaultAsync(c => c.Email == user.Email);

            var orderItems = await _db.OrderItems
                .Where(o => o.OrderNo == orderNo && o.Quantity > 0)
                .ToListAsync();

            foreach (var orderItem in orderItems)
            {
                var existingCartItem = await _db.BasketItems
                    .FirstOrDefaultAsync(b => b.StockID == orderItem.StockID && b.BasketID == customer.BasketID);

                if (existingCartItem == null)
                {
                    var newItem = new BasketItem
                    {
                        BasketID = customer.BasketID,
                        StockID = orderItem.StockID,
                        Quantity = orderItem.Quantity
                    };
                    _db.BasketItems.Add(newItem);
                }
                else
                {
                    existingCartItem.Quantity += orderItem.Quantity;
                    if (existingCartItem.Quantity > 100)
                    {
                        existingCartItem.Quantity = 100;
                    }
                    _db.BasketItems.Update(existingCartItem);
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToPage("/Checkout");
        }
    }
}
