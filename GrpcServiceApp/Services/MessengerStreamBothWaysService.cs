using Grpc.Core;

namespace GrpcServiceApp.Services
{
    public class MessengerStreamBothWaysService: MessengerBothWaysStreaming.MessengerBothWaysStreamingBase
    {
        // сообшения для отправки
        string[] messages = { "Привет", "Норм", "...", "Нет", "пока" };

        public async override Task StreamingBothWays(IAsyncStreamReader<RequestBothWay> requestStream, IServerStreamWriter<ResponseBothWay> responseStream, ServerCallContext context)
        {
            // считываем входящие сообщения в фоновой задаче
            var readTask = Task.Run(async () =>
            {
                await foreach (RequestBothWay message in requestStream.ReadAllAsync())
                {
                    Console.WriteLine($"Client: {message.Content}");
                }
            });

            foreach (var message in messages)
            {
                // Посылаем ответ, пока клиент не закроет поток
                if (!readTask.IsCompleted)
                {
                    await responseStream.WriteAsync(new ResponseBothWay { Content = message });
                    Console.WriteLine(message);
                    await Task.Delay(2000);
                }
            }
            await readTask; // ожидаем завершения задачи чтения
        }
    }
}
