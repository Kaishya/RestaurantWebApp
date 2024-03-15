using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestaurantWebApp.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using RestaurantWebApp.Models;
using Microsoft.EntityFrameworkCore;

public class PaymentModel : PageModel
{
    private readonly IConfiguration Configuration;
    private readonly RestaurantWebAppContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public IList<CheckoutItem> Items { get; private set; }
    public OrderHistory Order = new OrderHistory();

    public decimal Total;
    public long AmountPayable;

    public PaymentModel(IConfiguration configuration, RestaurantWebAppContext db, UserManager<IdentityUser> userManager)
    {
        Configuration = configuration;
        _db = db;
        _userManager = userManager;
    }

    public string StripePublishableKey => Configuration["Stripe:PublishableKey"];

    public decimal TotalAmount => Items.Sum(item => item.Quantity * item.Price);

    public int TotalAmountInCents => (int)(TotalAmount * 100);

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

    public IActionResult OnPost()
    {
        return RedirectToPage("/Success");
    }
}
