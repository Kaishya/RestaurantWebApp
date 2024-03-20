using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace RestaurantWebApp.Pages
{
    public class ContactModel : PageModel
    {
        [TempData]
        public string SuccessMessage { get; set; }
        [BindProperty]
        public string customerName { get; set; }
        [BindProperty]
        public string customerEmail { get; set; }
        [BindProperty]
        public string customerSubject { get; set; }
        [BindProperty]
        public string customerMessage { get; set; }
        const string appPassword = "xcdo yswf qadg cogz";
        public void OnGet()
        {        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Customer Contact Field Error", "Invalid data for customer contact form");
                return Page();
            }
            else
            {
                try
                {
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Credentials = new NetworkCredential("mathukhaicheiom@gmail.com", appPassword);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("mathukhaicheiom@gmail.com", "MathuKhaiChe Website Enquiry");
                    mailMessage.To.Add(new MailAddress("mathukhaicheiom@gmail.com")); /*Sends a real email*/

                    mailMessage.CC.Add(new MailAddress(customerEmail));
                    mailMessage.Body = "Hello,  \n" +
                        "A customer has contacted Mathu Khai Che:  \n" +
                        "Name: " + customerName + " \n" +
                        "Message: " + customerMessage + " \n";
                    mailMessage.Subject = "Mathu Khai Che Enquiry: " + customerSubject;
                    smtpClient.Send(mailMessage);
                    
                    SuccessMessage = "Sent, thank you for your message " + customerName + "!";
                    return RedirectToPage("/Contact");
                }
                catch
                {
                    ModelState.AddModelError("Customer Contact Field Error", "Invealid data for customer contact form");
                    return Page();
                }
            }
        }
    }
}
