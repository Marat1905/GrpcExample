using Google.Protobuf;
using Grpc.Core;
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

            #region унарный метод передачи
            //// создаем клиент
            //var client = new Translator.TranslatorClient(channel);

            //// отправляем каждое слово сервису для получения перевода
            //foreach (var word in words)
            //{
            //    // формируем сообщение для отправки
            //    Request request = new Request { Word = word };
            //    // отправляем сообщение и получаем ответ
            //    Response response = await client.TranslateAsync(request);
            //    // выводим слово и его перевод
            //    Console.WriteLine($"{response.Word} : {response.Translation}");
            //}
            #endregion


            #region унарный метод передачи DataTime
            //// создаем клиент
            //var client = new Inviter.InviterClient(channel);

            //// посылаем имя и получаем приглашение на мероприятие
            //var response = await client.inviteAsync(new RequestDate { Name = "Tom" });
            //var eventInvitation = response.Invitation;
            //var eventDateTime = response.Start.ToDateTime();
            //var eventDuration = response.Duration.ToTimeSpan();
            //// выводим данные на консоль
            //Console.WriteLine(eventInvitation);
            //Console.WriteLine($"Начало: {eventDateTime.ToString("dd.MM HH:mm")}   Длительность: {eventDuration.TotalHours} часа");
            #endregion

            #region Потоковая передача сервера
            //// создаем клиент
            //var client = new MessengerServerStream.MessengerServerStreamClient(channel);

            //// посылаем пустое сообщение и получаем набор сообщений
            //var serverData = client.StreamingFromServer(new RequestServerStream());

            //// получаем поток сервера
            //var responseStream = serverData.ResponseStream;
            //// с помощью итераторов извлекаем каждое сообщение из потока
            //while (await responseStream.MoveNext(new CancellationToken()))
            //{

            //    ResponseServerStream response = responseStream.Current;
            //    Console.WriteLine(response.Content);
            //}
            //// or
            ////await foreach (var response in responseStream.ReadAllAsync())
            ////{
            ////    Console.WriteLine(response.Content);
            ////}

            #endregion

            #region Потоковая передача клиента

            // данные для отправки
            string[] messages = { "Привет", "Как дела?", "Че молчишь?", "Ты че, спишь?", "Ну пока" };
            // создаем клиент
            var client = new MessengerClientStream.MessengerClientStreamClient(channel);

            var call = client.StreamingFromClient();

            // посылаем каждое сообщение
            foreach (var message in messages)
            {
                await call.RequestStream.WriteAsync(new RequestClientStream { Content = message });
            }
            // завершаем отправку сообшений в потоке
            await call.RequestStream.CompleteAsync();
            // получаем ответ сервера
            ResponseClientStream response = await call.ResponseAsync;
            Console.WriteLine($"Ответ сервера: {response.Content}");
            #endregion

            Console.ReadKey();
        }
    }
}
