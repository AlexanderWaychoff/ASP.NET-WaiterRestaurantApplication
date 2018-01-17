namespace WaiterRestaurantApplication.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualBasic.FileIO;
    using System.Web;
    using System.Web.Hosting;
    using System.Reflection;

    internal sealed class Configuration : DbMigrationsConfiguration<WaiterRestaurantApplication.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WaiterRestaurantApplication.Models.ApplicationDbContext context)
        {
            //Seed States table with all US States
            string seedFile = "~/CSV/SeedData/";//states.csv removed from end of seedFile
            string filePath = GetMapPath(seedFile);


            //alex's file path////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            filePath = @"C:\Users\Andross\Desktop\school_projects\C#\WaiterRestaurantApplication\WaiterRestaurantApplication\CSV\SeedData\";
            //end of alex's stuff/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            bool fileExists = File.Exists(filePath + "states.csv");
            if (fileExists)
            {
                List<State> states = new List<State>();
                using (TextFieldParser parser = new TextFieldParser(filePath + "states.csv"))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    State state;
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Any(x => x.Length == 0))
                        {
                            Console.WriteLine("We found an empty value in your CSV. Please check your file and try again.\nPress any key to return to main menu.");
                            Console.ReadKey(true);
                        }
                        state = new State();
                        state.Name = fields[0];
                        state.Abbreviation = fields[1];
                        states.Add(state);
                    }
                }
                context.States.AddOrUpdate(c => c.Abbreviation, states.ToArray());
                context.SaveChanges();
            }

            City Milwaukee = new City();
            Milwaukee.CityId = 1;
            Milwaukee.Name = "Milwaukee";

            ZipCode five3202 = new ZipCode();
            five3202.ZipCodeId = 1;
            five3202.Number = "53202";

            context.Cities.Add(Milwaukee);
            context.ZipCodes.Add(five3202);

            fileExists = File.Exists(filePath + "addresses.csv");
            if (fileExists)
            {
                List<Address> addresses = new List<Address>();
                using (TextFieldParser parser = new TextFieldParser(filePath + "addresses.csv"))
                {
                    int whileCount = 0;
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    Address address;
                    while (!parser.EndOfData)
                    {
                        whileCount += 1;
                        string[] fields = parser.ReadFields();
                        if (fields.Any(x => x.Length == 0))
                        {
                            Console.WriteLine("We found an empty value in your CSV. Please check your file and try again.\nPress any key to return to main menu.");
                            Console.ReadKey(true);
                        }
                        address = new Address();
                        address.AddressId = whileCount;
                        address.StreetOne = fields[0];
                        address.StreetTwo = fields[1];
                        address.CityId = Convert.ToInt32(fields[2]);
                        address.StateId = Convert.ToInt32(fields[3]);
                        address.ZipCodeId = Convert.ToInt32(fields[4]);
                        address.Lat = float.Parse(fields[5]);
                        address.Lng = float.Parse(fields[6]);
                        addresses.Add(address);
                    }
                }
                context.Addresses.AddOrUpdate(addresses.ToArray());
                context.SaveChanges();
            }

            Random random = new Random();
            fileExists = File.Exists(filePath + "restaurants.csv");
            if (fileExists)
            {
                List<Restaurant> restaurants = new List<Restaurant>();
                List<WaitRate> waitRates = new List<WaitRate>();
                using (TextFieldParser parser = new TextFieldParser(filePath + "restaurants.csv"))
                {
                    int whileCount = 0;
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    Restaurant restaurant;
                    WaitRate waitRate;
                    while (!parser.EndOfData)
                    {
                        whileCount += 1;
                        string[] fields = parser.ReadFields();
                        if (fields.Any(x => x.Length == 0))
                        {
                            Console.WriteLine("We found an empty value in your CSV. Please check your file and try again.\nPress any key to return to main menu.");
                            Console.ReadKey(true);
                        }
                        restaurant = new Restaurant();
                        waitRate = new WaitRate();
                        //add waitrate to list and save to context
                        restaurant.RestaurantId = whileCount;
                        restaurant.RestaurantName = fields[0];
                        restaurant.AddressId = Convert.ToInt32(fields[1]);
                        restaurant.OpenTime = fields[2];
                        restaurant.CloseTime = fields[3];
                        restaurant.IsOpen = true;
                        restaurant.PeopleBeforeWarning = Convert.ToInt32((fields[5]));
                        restaurant.GracePeriodMinutes = Convert.ToInt32(fields[6]);
                        restaurant.CurrentWaitMinutes = Convert.ToInt32(fields[7]);
                        restaurant.WaitRateId = whileCount;
                        waitRate.WaitRateId = whileCount;
                        waitRate.WateRatePercentage = random.Next(5, 95);
                        restaurants.Add(restaurant);
                        waitRates.Add(waitRate);
                    }
                }
                context.Restaurants.AddOrUpdate(restaurants.ToArray());
                context.WaitRate.AddOrUpdate(waitRates.ToArray());
                context.SaveChanges();
            }

            fileExists = File.Exists(filePath + "weatherConditions.csv");
            if (fileExists)
            {
                List<WeatherCondition> weatherConditions = new List<WeatherCondition>();
                using (TextFieldParser parser = new TextFieldParser(filePath + "weatherConditions.csv"))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    WeatherCondition weatherCondition;
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Any(x => x.Length == 0))
                        {
                            Console.WriteLine("We found an empty value in your CSV. Please check your file and try again.\nPress any key to return to main menu.");
                            Console.ReadKey(true);
                        }
                        weatherCondition = new WeatherCondition();
                        weatherCondition.WeatherConditionId = Convert.ToInt32(fields[0]);
                        weatherCondition.Temperature = Convert.ToInt32(fields[1]);
                        weatherCondition.WeatherDescription = fields[2];
                        weatherConditions.Add(weatherCondition);
                    }
                }
                context.WeatherConditions.AddOrUpdate(weatherConditions.ToArray());
                context.SaveChanges();
            }

            fileExists = File.Exists(filePath + "tableVisits.csv");
            if (fileExists)
            {
                List<TableVisit> tableVisits = new List<TableVisit>();
                using (TextFieldParser parser = new TextFieldParser(filePath + "tableVisits.csv"))
                {
                    //adjust datetime inputs here for week leading up to presentation (as long as the week doesn't cross into a month)
                    int year = 2018;
                    int month = 1;
                    int startDay = 12;
                    int endDay = 18;
                    int whileCount = startDay;
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    TableVisit tableVisit;
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Any(x => x.Length == 0))
                        {
                            Console.WriteLine("We found an empty value in your CSV. Please check your file and try again.\nPress any key to return to main menu.");
                            Console.ReadKey(true);
                        }
                        tableVisit = new TableVisit();
                        tableVisit.TableVisitId = Convert.ToInt32(fields[0]);
                        tableVisit.WaitMinutes = Convert.ToInt32(fields[1]);
                        tableVisit.WeatherConditionId = Convert.ToInt32(fields[2]);
                        if (Convert.ToInt32(fields[3]) == 1)
                        {
                            tableVisit.IsSatisfied = true;
                        }
                        else
                        {
                            tableVisit.IsSatisfied = false;
                        }
                        tableVisit.PartySize = Convert.ToInt32(fields[4]);
                        tableVisit.RestaurantId = Convert.ToInt32(fields[5]);
                        if (Convert.ToInt32(fields[6]) == 1)
                        {
                            tableVisit.IsHostEntry = true;
                        }
                        else
                        {
                            tableVisit.IsHostEntry = false;
                        }

                        tableVisit.IsWarned = true;
                        tableVisit.IsActive = false;
                        tableVisit.CreatedOn = new DateTime(year, month, whileCount);

                        tableVisits.Add(tableVisit);
                        whileCount += 1;
                        if(whileCount > endDay)
                        {
                            whileCount = startDay;
                        }
                    }

                }
                context.TableVisits.AddOrUpdate(tableVisits.ToArray());
                //context.SaveChanges();
            }

            int amountOfTableVisitsToAddForToday = 10;
            WeatherCondition weatherConditionToday = null;
            List<TableVisit> tableVisitsToday = new List<TableVisit>();
            int tableVisitsPrimaryKey = 22; //check tableVisits.csv, keep this count 1 above the entries in the csv.
            while (amountOfTableVisitsToAddForToday > 0)
            {
                amountOfTableVisitsToAddForToday -= 1;
                if(weatherConditionToday == null)
                {
                    weatherConditionToday = new WeatherCondition("Milwaukee");
                    context.WeatherConditions.Add(weatherConditionToday);
                    context.SaveChanges();
                }
                TableVisit tableVisit = new TableVisit();

                tableVisit.TableVisitId = tableVisitsPrimaryKey;
                tableVisitsPrimaryKey += 1;
                tableVisit.WaitMinutes = random.Next(10, 35);
                tableVisit.WeatherConditionId = 5;  //check weatherConditions.csv, keep this count 1 above the entries in the csv.
                if(tableVisit.WaitMinutes % 3 == 1)
                {
                    tableVisit.IsSatisfied = false;
                }
                else
                {
                    tableVisit.IsSatisfied = true;
                }
                tableVisit.PartySize = random.Next(2, 11);
                tableVisit.RestaurantId = 1;
                if(tableVisit.PartySize % 2 == 0)
                {
                    tableVisit.IsHostEntry = true;
                }
                else
                {
                    tableVisit.IsHostEntry = false;
                }
                tableVisit.IsWarned = true;
                tableVisit.IsActive = false;
                tableVisit.CreatedOn = DateTime.Now;

                tableVisitsToday.Add(tableVisit);
                //context.SaveChanges();
            }
            context.TableVisits.AddOrUpdate(tableVisitsToday.ToArray());

            //Seed the subscription types
            SubscriptionType monthlySubscription = new SubscriptionType();
            monthlySubscription.Name = "Monthly Subscription";
            monthlySubscription.Price = 9.99d;

            SubscriptionType annualSubscription = new SubscriptionType();
            annualSubscription.Name = "Annual Subscription";
            annualSubscription.Price = 110.00d;

            context.SubscriptionTypes.Add(monthlySubscription);
            context.SubscriptionTypes.Add(annualSubscription);

            context.SaveChanges();

        }

        private string GetMapPath(string seedFile)
        {
            if (HttpContext.Current != null)
                return HostingEnvironment.MapPath(seedFile);

            var absolutePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath; //was AbsolutePath but didn't work with spaces according to comments
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }


    }
}
