using SmartWorkSpasePLL.Dtos.Respons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ShowProfileResponse> ShowProfile(string userid);
        Task<bool> UpdateProfile(string userid, UpdateProfileResponse updateProfile);
        Task<bool> Updatepassword(string userid, UpdatePasswordResponse updatePassword);
    }
}
