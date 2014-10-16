namespace ElderberryLottery.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameCode
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique=true)]
        [MinLength(10)]
        [MaxLength(10)]
        public string Value { get; set; }

        public bool IsWinning { get; set; }

        //public virtual ApplicationUser User { get; set; }
    }
}
