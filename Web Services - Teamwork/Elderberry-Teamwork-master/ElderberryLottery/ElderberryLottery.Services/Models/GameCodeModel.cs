using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElderberryLottery.Services.Models
{
    public class GameCodeModel
    {
        [Required]      
        [MinLength(10)]
        [MaxLength(10)]
        public string Value { get; set; }
    }
}