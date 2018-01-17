namespace WaiterRestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        StreetOne = c.String(),
                        StreetTwo = c.String(),
                        CityId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        ZipCodeId = c.Int(nullable: false),
                        Lat = c.Single(),
                        Lng = c.Single(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.ZipCodes", t => t.ZipCodeId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.StateId)
                .Index(t => t.ZipCodeId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CityId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Abbreviation = c.String(),
                    })
                .PrimaryKey(t => t.StateId);
            
            CreateTable(
                "dbo.ZipCodes",
                c => new
                    {
                        ZipCodeId = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                    })
                .PrimaryKey(t => t.ZipCodeId);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        RestaurantId = c.Int(nullable: false, identity: true),
                        RestaurantName = c.String(),
                        AddressId = c.Int(nullable: false),
                        OpenTime = c.String(),
                        CloseTime = c.String(),
                        IsOpen = c.Boolean(nullable: false),
                        PeopleBeforeWarning = c.Int(nullable: false),
                        GracePeriodMinutes = c.Int(nullable: false),
                        CurrentWaitMinutes = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        WaitRateId = c.Int(nullable: false),
                        EstimatedWaitTime = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Subscription_SubscriptionId = c.Int(),
                    })
                .PrimaryKey(t => t.RestaurantId)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Subscriptions", t => t.Subscription_SubscriptionId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.WaitRates", t => t.WaitRateId, cascadeDelete: true)
                .Index(t => t.AddressId)
                .Index(t => t.UserId)
                .Index(t => t.WaitRateId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Subscription_SubscriptionId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsConfirmed = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Restaurant_RestaurantId = c.Int(),
                        Restaurant_RestaurantId1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_RestaurantId)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_RestaurantId1)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Restaurant_RestaurantId)
                .Index(t => t.Restaurant_RestaurantId1);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        SubscriptionId = c.Int(nullable: false, identity: true),
                        SubscriptionTypeId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        AutoRenewal = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SubscriptionId)
                .ForeignKey("dbo.SubscriptionTypes", t => t.SubscriptionTypeId, cascadeDelete: true)
                .Index(t => t.SubscriptionTypeId);
            
            CreateTable(
                "dbo.SubscriptionTypes",
                c => new
                    {
                        SubscriptionTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SubscriptionTypeId);
            
            CreateTable(
                "dbo.TableVisits",
                c => new
                    {
                        TableVisitId = c.Int(nullable: false, identity: true),
                        DinerName = c.String(),
                        DinerPhone = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        WaitMinutes = c.Int(nullable: false),
                        WeatherConditionId = c.Int(nullable: false),
                        IsHostEntry = c.Boolean(nullable: false),
                        IsSatisfied = c.Boolean(),
                        PartySize = c.Int(nullable: false),
                        IsWarned = c.Boolean(nullable: false),
                        GracePeriodStart = c.DateTime(),
                        IsNoShow = c.Boolean(),
                        IsActive = c.Boolean(nullable: false),
                        RestaurantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TableVisitId)
                .ForeignKey("dbo.WeatherConditions", t => t.WeatherConditionId, cascadeDelete: true)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.WeatherConditionId)
                .Index(t => t.RestaurantId);
            
            CreateTable(
                "dbo.WeatherConditions",
                c => new
                    {
                        WeatherConditionId = c.Int(nullable: false, identity: true),
                        Temperature = c.Int(nullable: false),
                        WeatherDescription = c.String(),
                    })
                .PrimaryKey(t => t.WeatherConditionId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        AmountPaid = c.Double(nullable: false),
                        Restaurant_RestaurantId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_RestaurantId)
                .Index(t => t.Restaurant_RestaurantId);
            
            CreateTable(
                "dbo.WaitRates",
                c => new
                    {
                        WaitRateId = c.Int(nullable: false, identity: true),
                        WaitRatePercentage = c.Int(),
                    })
                .PrimaryKey(t => t.WaitRateId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Restaurants", "WaitRateId", "dbo.WaitRates");
            DropForeignKey("dbo.Restaurants", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "Restaurant_RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.TableVisits", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.TableVisits", "WeatherConditionId", "dbo.WeatherConditions");
            DropForeignKey("dbo.Restaurants", "Subscription_SubscriptionId", "dbo.Subscriptions");
            DropForeignKey("dbo.Subscriptions", "SubscriptionTypeId", "dbo.SubscriptionTypes");
            DropForeignKey("dbo.AspNetUsers", "Restaurant_RestaurantId1", "dbo.Restaurants");
            DropForeignKey("dbo.AspNetUsers", "Restaurant_RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Restaurants", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Restaurants", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Addresses", "ZipCodeId", "dbo.ZipCodes");
            DropForeignKey("dbo.Addresses", "StateId", "dbo.States");
            DropForeignKey("dbo.Addresses", "CityId", "dbo.Cities");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Transactions", new[] { "Restaurant_RestaurantId" });
            DropIndex("dbo.TableVisits", new[] { "RestaurantId" });
            DropIndex("dbo.TableVisits", new[] { "WeatherConditionId" });
            DropIndex("dbo.Subscriptions", new[] { "SubscriptionTypeId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Restaurant_RestaurantId1" });
            DropIndex("dbo.AspNetUsers", new[] { "Restaurant_RestaurantId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Restaurants", new[] { "Subscription_SubscriptionId" });
            DropIndex("dbo.Restaurants", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Restaurants", new[] { "WaitRateId" });
            DropIndex("dbo.Restaurants", new[] { "UserId" });
            DropIndex("dbo.Restaurants", new[] { "AddressId" });
            DropIndex("dbo.Addresses", new[] { "ZipCodeId" });
            DropIndex("dbo.Addresses", new[] { "StateId" });
            DropIndex("dbo.Addresses", new[] { "CityId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.WaitRates");
            DropTable("dbo.Transactions");
            DropTable("dbo.WeatherConditions");
            DropTable("dbo.TableVisits");
            DropTable("dbo.SubscriptionTypes");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Restaurants");
            DropTable("dbo.ZipCodes");
            DropTable("dbo.States");
            DropTable("dbo.Cities");
            DropTable("dbo.Addresses");
        }
    }
}
