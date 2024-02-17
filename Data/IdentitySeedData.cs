
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace RestaurantWebApp.Data
{
    public class IdentitySeedData
    {
        public static async Task Initialize(RestaurantWebAppContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            string adminRole = "Admin";
            string memberRole = "Member";
            string password4all = "P@55word";

            if (await roleManager.FindByNameAsync(adminRole) == null) 
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            if(await roleManager.FindByNameAsync(memberRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(memberRole));
            }

            if(await userManager.FindByNameAsync("admin@ucm.ac.im")==null)
            {
                var user = new IdentityUser { 
                    UserName = "admin@ucm.ac.im",
                    Email = "admin@ucm.ac.im",
                    PhoneNumber = "06124 648200"
                };

                var result = await userManager.CreateAsync(user);
                if(result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password4all);
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }


            if (await userManager.FindByNameAsync("member@ucm.ac.im") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "member@ucm.ac.im",
                    Email = "member@ucm.ac.im",
                    PhoneNumber = "06124 648200"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password4all);
                    await userManager.AddToRoleAsync(user, memberRole);
                }
            }

            var allUsers = await userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                if (user.UserName != "admin@ucm.ac.im")
                {
                    await userManager.AddToRoleAsync(user, memberRole);
                }
            }

        }
    }
}
