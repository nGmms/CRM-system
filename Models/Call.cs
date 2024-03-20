using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMSystem.Models
{
    public class Call
    {
        [Key] //Primary Key
        public int CallNo { get; set; }
        
        [ForeignKey("Customer")]
        public int CustomerNo { get; set; }  // Foreign key to Customer

        public DateTime DateOfCall { get; set; }

        public TimeSpan TimeOfCall { get; set; }

        [StringLength(500)]
        public string Subject { get; set; }
    }
}