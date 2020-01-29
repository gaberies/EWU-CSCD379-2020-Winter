using AutoMapper;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    class UserService : EntityService<User>, IUserService
    {
        public UserService(ApplicationDbContext context, IMapper mapper) :
            base(context, mapper)
        { }
    }
}
