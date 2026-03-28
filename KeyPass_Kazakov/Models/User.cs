namespace KeyPass_Kazakov.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login {  get; set; }
        public string Password { get; set; }
        public DateTime? LastAuth { get; set; }
    }
}
