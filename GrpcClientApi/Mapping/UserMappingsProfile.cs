using AutoMapper;
using GrpcClientApi.Model;

namespace GrpcClientApi.Mapping
{
    public class UserMappingsProfile : Profile 
    {
        /// <summary>Профиль автомаппера для пользователя.</summary>
        public UserMappingsProfile()
        {
            CreateMap<UserReply, UserModel>();


            CreateMap<UserCreateModel, CreateUserRequest>();

            CreateMap<UserModel, UpdateUserRequest>();
        }
    }
}
