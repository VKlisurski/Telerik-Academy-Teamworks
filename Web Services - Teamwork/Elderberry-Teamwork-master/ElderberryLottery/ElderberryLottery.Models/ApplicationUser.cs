namespace ElderberryLottery.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<GameCode> gameCodes;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser()
        {
            this.gameCodes = new HashSet<GameCode>();
        }

        public string Firstname { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<GameCode> GameCodes
        {
            get
            {
                return this.gameCodes;
            }
            set
            {
                this.gameCodes = value;
            }
        }
    } 
}
