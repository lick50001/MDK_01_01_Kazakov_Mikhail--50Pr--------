using System.ComponentModel.DataAnnotations;

namespace KeyPass_Kazakov.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Login {  get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime? LastAuth { get; set; }

        public virtual ICollection<Storage> Storages { get; set; }
    }
}
