using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.Model
{
    public class UsersTest
    {
        public decimal ID { get; set; }
        public required string FIRSTNAME { get; set; }
        public required string LASTNAME { get; set; }
    }
}
