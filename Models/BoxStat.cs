using System.ComponentModel.DataAnnotations.Schema;

namespace SquirrelCannon.Models
{
    [NotMapped]
    public class BoxStat
    {
        public int Box { get; set; }
        public int Count { get; set; }
    }

}
