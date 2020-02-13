using AutoMapper;
using SecretSanta.Data;
using System.Reflection;

namespace SecretSanta.Business
{
    public class AutomapperConfigurationProfile : Profile
    {
        public AutomapperConfigurationProfile()
        {
            CreateMap<User, Dto.User>();
            CreateMap<Dto.UserInput, User>();
            CreateMap<User, Dto.UserInput>();

            CreateMap<Group, Dto.Group>();
            CreateMap<Dto.GroupInput, Group>();
            CreateMap<Group, Dto.GroupInput>();

            CreateMap<Gift, Dto.Gift>();
            CreateMap<Dto.GiftInput, Gift>();
            CreateMap<Gift, Dto.GiftInput>();
        }

        public static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
