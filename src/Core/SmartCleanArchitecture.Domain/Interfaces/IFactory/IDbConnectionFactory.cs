using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Domain.Interfaces.IFactory
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
