using AutoMapper;
using Crud;
using GrpcServerApp.Entities;

namespace GrpcServerApp.Mapping
{
    public class UserMappingsProfile : Profile
    {
        /// <summary>Профиль автомаппера для пользователя.</summary>
        public UserMappingsProfile()
        {
            CreateMap<User, UserReply>();

            CreateMap<CreateUserRequest, User>();

            CreateMap<UpdateUserRequest, User>();
        }
    }
}
