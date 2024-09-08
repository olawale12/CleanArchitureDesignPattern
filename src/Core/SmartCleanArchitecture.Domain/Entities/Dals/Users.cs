using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Domain.Entities.Dals
{
    public class Users: Entity<Guid>, ISoftDelete
    {

        public  string UserName { get; set; }
        [Required]
        public  string PhoneNo { get; set; }
        [Required]
        public  string FirstName { get; set; }
        [Required]
        public  string LastName { get; set; }
        [Required]
        public  string Dob { get; set; }
        [Required]
        public  string PasswordHash { get; set; }
        [Required]
        public  string PasswordSalt { get; set; }
        [Required]
        public  string Email { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}
