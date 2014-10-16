using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using ElderberryLottery.Models;
using ElderberryLotttery.Data;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using ElderberryLottery.Services.Models;

namespace ElderberryLottery.Services.Controllers
{
    [Authorize]
    public class GameCodeController : ApiController
    {
        private IElderberryLotteryData data;

        public GameCodeController(IElderberryLotteryData data)
        {
            this.data = data;
        }

        [HttpGet]
        public IHttpActionResult GetUserCodes()
        {
            var userId = this.GetUserId();

            var currentUser = this.data.Users.All().FirstOrDefault(x => x.Id == userId);

            var userCodes = currentUser.GameCodes;

            return Ok(userCodes);
        }

        [HttpPost]
        public IHttpActionResult Create(GameCodeModel model)
        {
            var userId = this.GetUserId();

            var existingCode = this.data.GameCodes.All().FirstOrDefault(x => x.Value == model.Value);

            if (existingCode == null)
            {
                return BadRequest("Invalid code!");
            }

            if (this.data.Users
                .All()
                .Any(u => u.GameCodes                    
                           .Any(c => c.Value == existingCode.Value)))
            {
                return BadRequest("This code is already used!");
            }

            var existingUser = this.data.Users.All().FirstOrDefault(user => user.Id == userId);
            if (!existingUser.GameCodes.Contains(existingCode))
            {
                existingUser.GameCodes.Add(existingCode);
            }


            this.data.SaveChanges();

            if (existingCode.IsWinning)
            {
                return Ok("You win!");
            }

            return Ok("You did not win!");
        }

        private string GetUserId()
        {
            return Thread.CurrentPrincipal.Identity.GetUserId();
        }

    }
}
