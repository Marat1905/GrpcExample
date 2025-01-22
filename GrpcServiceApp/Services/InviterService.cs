using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GrpcServiceApp.Services
{
    public class InviterService : Inviter.InviterBase
    {
        public override Task<ResponseDate> invite(RequestDate request, ServerCallContext context)
        {
            //начало мероприятия
            var eventDateTime = DateTime.UtcNow.AddDays(1);
            //Длительность мероприятия - условно 2 часа
            var eventDuration = TimeSpan.FromHours(2);
            // отправляем ответ
            return Task.FromResult(new ResponseDate
            {
                Invitation = $"{request.Name}, приглашаем Вас посетить мероприятие",
                Start = Timestamp.FromDateTime(eventDateTime),
                Duration = Duration.FromTimeSpan(eventDuration)
            });
        }
    }
}
