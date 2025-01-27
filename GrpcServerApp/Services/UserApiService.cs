using AutoMapper;
using Crud;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServerApp.Context;
using GrpcServerApp.Entities;

namespace GrpcServerApp.Services
{
    public class UserApiService : UserService.UserServiceBase
    {
        private readonly ApplicationContext _db;
        private readonly IMapper _mapper;

        public UserApiService(ApplicationContext db , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        // отправляем список пользователей
        public override Task<ListReply> ListUsers(Empty request, ServerCallContext context)
        {
            var listReply = new ListReply();    // определяем список
                                                // преобразуем каждый объект User в объект UserReply
            listReply.Users.AddRange(_mapper.Map<ICollection<UserReply>>(_db.Users));
            return Task.FromResult(listReply);
        }
        // отправляем одного пользователя по id
        public override async Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var user = await _db.Users.FindAsync(request.Id);
            // если пользователь не найден, генерируем исключение
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            return await Task.FromResult(_mapper.Map<UserReply>(user));
        }
        // добавление пользователя
        public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            // формируем из данных объект User и добавляем его в список users
            var user = _mapper.Map<User>(request);
            //var user = new User { Name = request.Name, Age = request.Age };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            var reply = _mapper.Map<UserReply>(user);
            return await Task.FromResult(reply);
        }
        // обновление пользователя
        public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var user = await _db.Users.FindAsync(request.Id);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            // обновляем даннные
            _mapper.Map(request, user);

            await _db.SaveChangesAsync();
            var reply = _mapper.Map<UserReply>(user);
            return await Task.FromResult(reply);
        }
        // удаление пользователя
        public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var user = await _db.Users.FindAsync(request.Id);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            // удаляем пользователя из бд
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            var reply = _mapper.Map<UserReply>(user);
            return await Task.FromResult(reply);
        }
    }
}
