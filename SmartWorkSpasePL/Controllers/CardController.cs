using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWorkSpasePLL.Services.Interfaces;
using SmartWorkSpasePLL.Utilise;
using System.Security.Claims;

namespace SmartWorkSpasePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        public CardController(ICardService cardService)
        {
             _cardService = cardService;
        }

        [HttpGet("GetAllCards")]
        public async Task<IActionResult> GetAllCards()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) 
                return NotFound();

            var cards = await _cardService.GetAllCardAsync(userid);

            if (cards == null || !cards.Any())
            {
                return NotFound("No cards found for this user.");
            }

            return Ok(cards);
        }

        [HttpGet("GetCardById/{id}")]
        public async Task<IActionResult> GetCardById(int id)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
                return NotFound();

            var card = await _cardService.GetCardById(id, userid);

            if (card == null)
            {
                return NotFound("Card not found.");
            }

            return Ok(card);
        }

        [HttpPut("settle")]
        public async Task<IActionResult> SettleCards()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
                return NotFound();

            var result = await _cardService.settled(userid);

            if (!result)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Failed to settle cards."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Cards have been settled successfully."
            });
        }

        
        [HttpPut("cancel/{cardId:int}")]
        [Authorize(Roles =$"{DS.ADMIN_ROLE}, {DS.HR_ROLE}")]
        public async Task<IActionResult> CancelCard(int cardId)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userid == null)
                return NotFound();

            var result = await _cardService.CancelCardAsync(cardId,userid);

            if (!result.Item1)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Card not found or already cancelled And NotAllowed"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Card has been cancelled successfully."
            });
        }
    }
}
