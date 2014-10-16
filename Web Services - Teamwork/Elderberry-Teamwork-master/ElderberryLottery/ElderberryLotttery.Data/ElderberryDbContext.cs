namespace ElderberryLotttery.Data
{
    using ElderberryLottery.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.Entity;
    using ElderberryLotttery.Data.Migrations;

    public class ElderberryDbContext : IdentityDbContext<ApplicationUser>
    {
        public ElderberryDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ElderberryDbContext, Configuration>());
            //Database.SetInitializer<ElderberryDbContext>(null);
        }

        public static ElderberryDbContext Create()
        {
            return new ElderberryDbContext();
        }

       public IDbSet<GameCode> GameCodes { get; set; }

       //public IDbSet<ApplicationUser> Users { get; set; }
    }
}
