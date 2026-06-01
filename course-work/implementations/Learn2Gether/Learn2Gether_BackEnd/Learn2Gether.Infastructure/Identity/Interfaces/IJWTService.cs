using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Infastructure.Identity.Interfaces
{
    public interface IJWTService
    {
        string GenerateToken(string username, string email, IList<string> roles);
    }
}
