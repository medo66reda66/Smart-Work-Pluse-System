using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IEnumerable<Claim> claims);
    }
}
