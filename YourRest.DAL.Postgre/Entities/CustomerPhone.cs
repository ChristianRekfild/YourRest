using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.DAL.Postgre.Entities
{
    public class CustomerPhone
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        [Comment("One of the customers Phones")]
        public string Phone { get; set; }
        [Required]
        [Comment("If false - this number is not active. If true - you may call on this Number")]
        public bool IsActive { get; set; }
    }
}
