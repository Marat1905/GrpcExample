using Grpc.Net.Client;
using GrpcClientApp;

namespace GprcClientApp
{
    internal class Program
    {
        static async  Task Main(string[] args)
        {
            // список слов для получения перевода
            var words = new List<string>() { "red", "yellow", "green" };

            // создаем канал для обмена сообщениями с сервером
            // параметр - адрес сервера gRPC
            using var channel = GrpcChannel.ForAddress("https://localhost:7266");

            // создаем клиент
            var client = new Translator.TranslatorClient(channel);

            // отправляем каждое слово сервису для получения перевода
            foreach (var word in words)
            {
                // формируем сообщение для отправки
                Request request = new Request { Word = word };
                // отправляем сообщение и получаем ответ
                Response response = await client.TranslateAsync(request);
                // выводим слово и его перевод
                Console.WriteLine($"{response.Word} : {response.Translation}");
            }

            Console.ReadKey();
        }
    }
}
