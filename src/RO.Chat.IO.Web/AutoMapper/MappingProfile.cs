using AutoMapper;

namespace RO.Chat.IO.Web.AutoMapper
{

    public class AutoMapperConfiguration
    {
        public static IMapper RegisterMappings()
        {
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });
            return configuration.CreateMapper();
        }
    }



}
