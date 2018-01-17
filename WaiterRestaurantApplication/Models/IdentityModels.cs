using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace WaiterRestaurantApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //Extended Properties
        public ICollection<Restaurant> Restaurants { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.Restaurant> Restaurants { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.Subscription> Subscriptions { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.SubscriptionType> SubscriptionTypes { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.State> States { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.ZipCode> ZipCodes { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.TableVisit> TableVisits { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.WeatherCondition> WeatherConditions { get; set; }

        public System.Data.Entity.DbSet<WaiterRestaurantApplication.Models.WaitRate> WaitRate { get; set; }
    }
}