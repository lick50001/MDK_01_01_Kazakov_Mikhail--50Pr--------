using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyPass_Kazakov.Models
{
    public class StorageDto
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Login {  get; set; }
        public string Password { get; set; }
    }
}
