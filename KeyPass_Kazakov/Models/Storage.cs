using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KeyPass_Kazakov.Models
{
    public class Storage
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
