using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Stripe;
using WaiterRestaurantApplication.Models;

[assembly: OwinStartupAttribute(typeof(WaiterRestaurantApplication.Startup))]
namespace WaiterRestaurantApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            //NOTE: MOVE THIS TO SOMETHING LIKE A appsettings.json FILE that is in .gitignore! SHOULD NOT PUT THIS ON GITHUB!
            StripeConfiguration.SetApiKey("sk_test_A1hCCloeZ40NRJNUNakK39Cs");
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  
                var user = new ApplicationUser();
                user.UserName = "BigBoss";
                user.Email = "redsa7777@yahoo.com";

                string userPWD = "Im_In_Charge77";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("RestaurantManager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "RestaurantManager";
                roleManager.Create(role);

                //Here we create a Default Restaurant Manager                  
                var user = new ApplicationUser();
                user.UserName = "AleHouseGuy";
                user.Email = "beers@ale-house.com";

                string userPWD = "Password123!";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "RestaurantManager");

                }
                user = new ApplicationUser();
                user.UserName = "GoudaMan";
                user.Email = "Cheese@italian.com";

                userPWD = "Password123!";

                chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "RestaurantManager");

                }

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("RestaurantEmployee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "RestaurantEmployee";
                roleManager.Create(role);

                //Here we create a Default Restaurant Employee                  
                var user = new ApplicationUser();
                user.UserName = "EmployeeGuy1";
                user.Email = "employee@ale-house.com";

                string userPWD = "Password123!";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "RestaurantEmployee");

                }
            }

            if (!roleManager.RoleExists("Diner"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Diner";
                roleManager.Create(role);
            }
        }
    }
}
