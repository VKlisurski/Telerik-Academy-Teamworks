namespace ElderberryLotttery.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ElderberryLottery.Models;
    using ElderberryLotttery.Data.Repositories;


    public class ElderberryLotteryData : IElderberryLotteryData
    {

        private DbContext context;
        private IDictionary<Type, object> repositories;

        public ElderberryLotteryData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public Repositories.IRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetRepository<ApplicationUser>();
            }
        }

        public Repositories.IRepository<GameCode> GameCodes
        {
            get
            {
                return this.GetRepository<GameCode>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(Repository<T>), context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
