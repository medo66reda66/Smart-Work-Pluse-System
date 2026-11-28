using Microsoft.AspNetCore.Identity;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Repositories.Interfaces;
using SmartWorkSpasePLL.Dtos.Respons;
using SmartWorkSpasePLL.Services.Interfaces;
using SmartWorkSpasePLL.Utilise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class CardService : ICardService
    {
        private readonly IRepository<Card> _card;
        private readonly UserManager<Applicationuser> _userManager;
        private readonly IRepository<Notification> _notification;

        public CardService(IRepository<Card> card, UserManager<Applicationuser> userManager, IRepository<Notification> notification)
        {
            _card = card;
            _userManager = userManager;
            _notification = notification;
        }

        public async Task<List<CardResponse>> GetAllCardAsync(string userid)
        {
            var allcards =await _card.GetAllAsync(e=>e.ApplicationUserId == userid && e.Status == CardStatus.Active);
            if (allcards == null)
            {
                return null;
            }

            return allcards.Select(r => new CardResponse
            {
                ApplicationUserId = userid,
                Color = r.Color,
                Source = r.Source,
                Status = r.Status,
                IssuedAt = r.IssuedAt,
            }).ToList();
        }
        public async Task<CardResponse> GetCardById(int id ,  string userid)
        {
            var allcards = await _card.GetByIdAsync(e=>e.Id == id && e.ApplicationUserId == userid && e.Status == CardStatus.Active);
            if (allcards == null)
            {
                return null;
            }

            return new CardResponse
            {
                ApplicationUserId = userid,
                Color = allcards.Color,
                Source = allcards.Source,
                Status = allcards.Status,
                IssuedAt = allcards.IssuedAt,
            };
        }
        public async Task<bool> settled(string userid)
        {
            var allcards = await _card.GetAllAsync(e => e.ApplicationUserId == userid 
            &&(e.Color == CardColor.Green || e.Color == CardColor.Blue ) &&e.Status == CardStatus.Active);

            var GreenCard = allcards.Where(e => e.Color == CardColor.Green).ToList();
            var CountGreenCard = GreenCard.Count();
            if (GreenCard == null)
            {
                return false;
            }

            var BlueCard = allcards.Where(e => e.Color == CardColor.Blue).ToList();
            var CountBlueCard = BlueCard.Count();
            if (BlueCard == null)
            {
                return false;
            }

            int CountToSettle = Math.Min(CountGreenCard, CountBlueCard);

            // اختيار الكروت
            var SettledGreen = GreenCard.Take(CountToSettle).ToList();
            var SettledBlue = BlueCard.Take(CountToSettle).ToList();

            // تغيير حالة الكروت
            foreach (var item in SettledGreen)
            {
                item.Status = CardStatus.Settled;
            }

            foreach (var item in SettledBlue)
            {
                item.Status = CardStatus.Settled;
            }
            await _card.SaveChangesAsync();

            var HrUser =await _userManager.GetUsersInRoleAsync(DS.HR_ROLE);
            var user =await _userManager.FindByIdAsync(userid);
            foreach (var item in HrUser)
            {
                var notif = new Notification
                {
                    ApplicationUserId = item.Id,
                    Type = "CardSettlement",
                    Message = $"Employee {user!.FullName} has successfully settled " +
                 $"{CountToSettle} Green Cards with {CountToSettle} Blue Cards.",
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                    RelatedUserId = userid,
                };
                _notification.Add(notif);
            }
            await _notification.SaveChangesAsync();

            return true;
        }

        //HR
        public async Task<(bool, string)> CancelCardAsync(int cardId,string userid)
        {
            var card = await _card.GetByIdAsync(e => e.Id == cardId && e.Status == CardStatus.Active);

            if (card == null)
            {
                return (false, "Card not found or it is already cancelled or settled.");
            }

            if (userid == card.ApplicationUserId)
            {
                return (false, "You are not allowed to cancel your own card.");
            }
            card.Status = CardStatus.Cancelled;
            await _card.SaveChangesAsync();


            return (true, "Card has been cancelled successfully.");
        }
    }
}
