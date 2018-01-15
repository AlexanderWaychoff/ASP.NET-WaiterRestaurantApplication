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
            string seedFile = "~/CSV/SeedData/states.csv";
            string filePath = GetMapPath(seedFile);
            //alex's file path
            //filePath = @"C:\Users\Andross\Desktop\school_projects\C#\WaiterRestaurantApplication\WaiterRestaurantApplication\CSV\SeedData\states.csv";
            bool fileExists = File.Exists(filePath);
            if (fileExists)
            {
                List<State> states = new List<State>();
                using (TextFieldParser parser = new TextFieldParser(filePath))
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

                //  This method will be called after migrating to the latest version.

                //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
                //  to avoid creating duplicate seed data. E.g.
                //
                //    context.People.AddOrUpdate(
                //      p => p.FullName,
                //      new Person { FullName = "Andrew Peters" },
                //      new Person { FullName = "Brice Lambson" },
                //      new Person { FullName = "Rowan Miller" }
                //    );
                //
            }
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
