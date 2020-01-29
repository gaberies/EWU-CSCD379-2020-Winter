using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;
using System.Threading.Tasks;

namespace SecretSanta.Business
{
    public class GiftService : EntityService<Gift>, IGiftService
    {
        public GiftService(ApplicationDbContext context, IMapper mapper):
            base(context, mapper)
        { }

        public override async Task<Gift> FetchByIdAsync(int id)
        {
            Gift gift = await ApplicationDbContext.Set<Gift>().SingleAsync(item => item.Id == id);
            gift.User = await ApplicationDbContext.Set<User>().SingleAsync(item => item.Id == gift.UserId);
            return gift;
        }
    }
}
