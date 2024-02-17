using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RestaurantWebApp.Pages
{
    public class ContactModel : PageModel
    {
        [TempData]
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // You can perform any necessary logic here
        }

        public IActionResult OnPost()
        {
            // Perform form validation if needed

            // Assuming the form is valid, set a custom success message
            SuccessMessage = "Sent, Thank you for your message!";

            // Redirect to the same page
            return RedirectToPage("/Contact");
        }
    }
}
