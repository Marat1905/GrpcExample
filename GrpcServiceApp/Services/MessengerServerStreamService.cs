using Grpc.Core;

namespace GrpcServiceApp.Services
{
    public class MessengerServerStreamService : MessengerServerStream.MessengerServerStreamBase
    {
        string[] messages = { "Привет", "Как дела?", "Че молчишь?", "Ты че, спишь?", "Ну пока" };

        public async override Task StreamingFromServer(RequestServerStream request, IServerStreamWriter<ResponseServerStream> responseStream, ServerCallContext context)
        {
            foreach (var message in messages)
            {
                await responseStream.WriteAsync(new ResponseServerStream { Content = message });
                // для имитации работы делаем задержку в 1 секунду
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}
