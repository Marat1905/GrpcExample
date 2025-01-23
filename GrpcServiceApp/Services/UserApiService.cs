using CrudExample;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
namespace GrpcServiceApp.Services
{
    public class UserApiService:UserService.UserServiceBase
    {
        ApplicationContext db;
        public UserApiService(ApplicationContext db)
        {
            this.db = db;
        }
        // отправляем список пользователей
        public override Task<ListReply> ListUsers(Empty request, ServerCallContext context)
        {
            var listReply = new ListReply();    // определяем список
                                                // преобразуем каждый объект User в объект UserReply
            var userList = db.Users.Select(item => new UserReply { Id = item.Id, Name = item.Name, Age = item.Age }).ToList();
            listReply.Users.AddRange(userList);
            return Task.FromResult(listReply);
        }
        // отправляем одного пользователя по id
        public override async Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var user = await db.Users.FindAsync(request.Id);
            // если пользователь не найден, генерируем исключение
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            UserReply userReply = new UserReply() { Id = user.Id, Name = user.Name, Age = user.Age };
            return await Task.FromResult(userReply);
        }
        // добавление пользователя
        public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            // формируем из данных объект User и добавляем его в список users
            var user = new User { Name = request.Name, Age = request.Age };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            var reply = new UserReply() { Id = user.Id, Name = user.Name, Age = user.Age };
            return await Task.FromResult(reply);
        }
        // обновление пользователя
        public override async Task<UserReply> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var user = await db.Users.FindAsync(request.Id);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            // обновляем даннные
            user.Name = request.Name;
            user.Age = request.Age;
            await db.SaveChangesAsync();
            var reply = new UserReply() { Id = user.Id, Name = user.Name, Age = user.Age };
            return await Task.FromResult(reply);
        }
        // удаление пользователя
        public override async Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            var user = await db.Users.FindAsync(request.Id);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            // удаляем пользователя из бд
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            var reply = new UserReply() { Id = user.Id, Name = user.Name, Age = user.Age };
            return await Task.FromResult(reply);
        }
    }
}
