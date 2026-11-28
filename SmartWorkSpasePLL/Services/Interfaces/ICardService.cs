using SmartWorkSpaseADL.Entities;
using SmartWorkSpasePLL.Dtos.Respons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface ICardService
    {
        Task<List<CardResponse>> GetAllCardAsync(string userid);

        Task<CardResponse> GetCardById(int id, string userid);
        Task<bool> settled(string userid);
        Task<(bool, string msg)> CancelCardAsync(int cardId, string userid);
    }
}
