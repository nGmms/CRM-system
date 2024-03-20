using System.ComponentModel.DataAnnotations;

namespace CRMSystem.Models
{
    public class Customer
    {
        [Key] //Primary Key
        public int CustomerNo { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        [StringLength(25)]
        public string Address { get; set; }

        public string PostCode { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; }

    }

}