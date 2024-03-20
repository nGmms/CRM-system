using System.ComponentModel.DataAnnotations;

namespace CRMSystem.Models
{
    public class User
    {
        [Key] //Primary Key
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }  // "Employee" or "Manager"
    }
}
