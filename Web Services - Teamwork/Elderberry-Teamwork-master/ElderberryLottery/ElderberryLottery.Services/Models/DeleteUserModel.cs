using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElderberryLottery.Services.Models
{
    public class DeleteUserModel
    {
        [Required]
        public string UserName { get; set; }
    }
}