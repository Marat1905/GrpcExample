using Grpc.Core;

namespace GrpcServiceApp.Services
{
    public class MessagerHeaderService: MessengerHeader.MessengerHeaderBase
    {

        public override Task<ResponseHeader> SendMessageHeader(RequestHeader request, ServerCallContext context)
        {
            //Получаем все заголовки запроса
            foreach (var header in context.RequestHeaders)
            {
                Console.WriteLine($" {header.Key}: {header.Value}");//Получаем ключ и значение заголовка
            }

            // получаем один заголовок по названию - user-agent
            var userAgent = context.RequestHeaders.GetValue("user-agent");

            //отправляем ответ
            return Task.FromResult(new ResponseHeader
            {
                Content = userAgent,
            });
        }
    }
}
