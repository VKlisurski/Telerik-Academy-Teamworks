namespace ElderberryLotttery.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ElderberryLottery.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ElderberryDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ElderberryDbContext context)
        {
            if (context.GameCodes.Any() == false)
            {
                this.SeedGameNumbers(context);
            }

            if (context.Roles.Any() == false)
            {
                this.SeedRolesAndUsers(context);
            }
        }

        private void SeedRolesAndUsers(ElderberryDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            roleManager.Create(new IdentityRole("Administrator"));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser { UserName = "admin" };
            userManager.Create(user, "admin321");
            userManager.AddToRole(user.Id, "Administrator");
        }

        private void SeedGameNumbers(ElderberryDbContext context)
        {
            context.GameCodes.Add(new GameCode
            {
                Value = "1232134598",
                IsWinning = false
            });

            context.GameCodes.Add(new GameCode
            {
                Value = "9999999999",
                IsWinning = true
            });

            context.GameCodes.Add(new GameCode
            {
                Value = "1234512345",
                IsWinning = false
            });

            context.GameCodes.Add(new GameCode
            {
                Value = "1231231231",
                IsWinning = true
            });

            context.GameCodes.Add(new GameCode
            {
                Value = "1111111111",
                IsWinning = false
            });

            context.GameCodes.Add(new GameCode
            {
                Value = "9182736451",
                IsWinning = false
            });

            context.GameCodes.Add(new GameCode
            {
                Value = "0000001111",
                IsWinning = false
            });
        }
    }
}
