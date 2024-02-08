using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantWebApp.Data;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Models;
using Microsoft.AspNetCore.Authorization;
//using Stripe.Checkout;
//using Stripe;


namespace RestaurantWebApp.Pages
{
    [Authorize(Roles = "Admin, Member")]

    public class CheckoutModel : PageModel
    {

        private readonly RestaurantWebAppContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;  // Add IConfiguration

        public IList<CheckoutItem> Items { get; private set; }
        public OrderHistory Order = new OrderHistory();

        public decimal Total;
        public long AmountPayable;
        public CheckoutModel(RestaurantWebAppContext db, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _configuration = configuration;
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
            Total = 0;
            foreach (var item in Items)
            {
                Total += (item.Quantity * item.Price);
            }
            AmountPayable = (long)Total;

        }

        public async Task<IActionResult> OnPostBuyAsync()
        {
           // try
            //{
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

                CheckoutCustomer customer = await _db
                    .CheckoutCustomers
                    .FindAsync(user.Email);
                var basketItems =
                    _db.BasketItems
                    .FromSqlRaw("SELECT * From BasketItems WHERE BasketID = {0}", customer.BasketID).ToList();
                foreach (var item in basketItems)
                {
                    OrderItem oi = new OrderItem
                    {
                        OrderNo = Order.OrderNo,
                        StockID = item.StockID,
                        Quantity = item.Quantity
                    };
                    _db.OrderItems.Add(oi);
                    _db.BasketItems.Remove(item);
                }
                await _db.SaveChangesAsync();
/*
				StripeConfiguration.ApiKey = _configuration["Stripe:StripeSecretKey"];

				var options = new SessionCreateOptions
				{
					PaymentMethodTypes = new List<string> { "card" },
					LineItems = new List<SessionLineItemOptions>
					{
                        // Add your line items here (products in the order)
                        new SessionLineItemOptions
						{
							Name = "Product Name",
							Amount = AmountPayable * 100, // Convert to cents
                            Currency = "usd",
							Quantity = 1,
						}
					},
					SuccessUrl = "/Index", // Replace with your success URL
					CancelUrl = "/Index",   // Replace with your cancel URL
				};

				var service = new SessionService();
				var session = service.Create(options);

				// Redirect the user to the Stripe Checkout page
				return Redirect(session.Url);
			}
            catch (Exception ex)
            {
				Console.WriteLine($"Exception: {ex.Message}");
				Console.WriteLine($"StackTrace: {ex.StackTrace}");
				return RedirectToPage("/Menu/Menu"); // Replace with your error page}
            }*/
            return RedirectToPage("/Index");

        }
    }
}
