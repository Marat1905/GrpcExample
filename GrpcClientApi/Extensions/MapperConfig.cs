using AutoMapper;
using GrpcClientApi.Mapping;

namespace GrpcClientApi.Extensions
{
    public static class MapperConfig
    {
        public static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMappingsProfile>();

            });
            //configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}
