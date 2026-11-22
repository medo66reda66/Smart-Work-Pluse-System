using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Dtos.Respons.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface IApplicationuserAuthServise
    {
         Task<bool> RegisterUserAsync(RegisterUserRequest request);

        Task<LogingResult> LoginUserAsync(LoginRequest loginRequest);

    }
}
