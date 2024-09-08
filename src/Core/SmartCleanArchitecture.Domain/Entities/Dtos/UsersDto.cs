using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Domain.Entities.Dtos
{
    public class UsersDto
    {
        public  string Id { get; set; }
        public  string UserName { get; set; }
        public  string PhoneNo { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string Dob { get; set; }
        public  string Email { get; set; }
        public  DateTime LastLogin { get; set; }
    }
}
