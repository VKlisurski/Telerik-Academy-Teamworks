namespace ElderberryLotttery.Data
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElderberryLottery.Models;
using ElderberryLotttery.Data.Repositories;

    public interface IElderberryLotteryData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<GameCode> GameCodes { get; }

        int SaveChanges();
    }
}
