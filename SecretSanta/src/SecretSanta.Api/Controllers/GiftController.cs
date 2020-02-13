using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : BaseApiController<Business.Dto.Gift, Business.Dto.GiftInput>
    {
        public GiftController(IGiftService giftService)
            : base(giftService) { }
    }
}