using CrudExample;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcClientApp;
using ServerStream;

namespace GprcClientApp
{
    internal class Program
    {
        static async  Task Main(string[] args)
        {
            // список слов для получения перевода
            var words = new List<string>() { "red", "yellow", "green" };

            // данные для отправки
            string[] messages = { "Привет", "Как дела?", "Че молчишь?", "Ты че, спишь?", "Ну пока" };

            // создаем канал для обмена сообщениями с сервером
            // параметр - адрес сервера gRPC
            using var channel = GrpcChannel.ForAddress("http://localhost:5129");
            Console.WriteLine("scsx");

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
            // создаем клиент
            var client = new MessengerServerStream.MessengerServerStreamClient(channel);

            // посылаем пустое сообщение и получаем набор сообщений
            var serverData = client.StreamingFromServer(new RequestServerStream());

            // получаем поток сервера
            var responseStream = serverData.ResponseStream;
            // с помощью итераторов извлекаем каждое сообщение из потока
            while (await responseStream.MoveNext(new CancellationToken()))
            {

                ResponseServerStream response = responseStream.Current;
                Console.WriteLine(response.Content);
            }
            // or
            //await foreach (var response in responseStream.ReadAllAsync())
            //{
            //    Console.WriteLine(response.Content);
            //}

            #endregion

            #region Потоковая передача клиента


            //// создаем клиент
            //var client = new MessengerClientStream.MessengerClientStreamClient(channel);

            //var call = client.StreamingFromClient();

            //// посылаем каждое сообщение
            //foreach (var message in messages)
            //{
            //    await call.RequestStream.WriteAsync(new RequestClientStream { Content = message });
            //}
            //// завершаем отправку сообшений в потоке
            //await call.RequestStream.CompleteAsync();
            //// получаем ответ сервера
            //ResponseClientStream response = await call.ResponseAsync;
            //Console.WriteLine($"Ответ сервера: {response.Content}");
            #endregion

            #region Двунаправленняя потоковая передача
            //// создаем клиент
            //var client = new MessengerBothWaysStreaming.MessengerBothWaysStreamingClient(channel);

            //// получаем объект AsyncDuplexStreamingCall
            //var call = client.StreamingBothWays();

            //var readTask = Task.Run(async () =>
            //{
            //    await foreach (var response in call.ResponseStream.ReadAllAsync())
            //    {
            //        Console.WriteLine($"Server: {response.Content}");
            //    }
            //});
            //foreach (var message in messages)
            //{
            //    await call.RequestStream.WriteAsync(new RequestBothWay { Content = message });
            //    Console.WriteLine(message);
            //    await Task.Delay(2000);
            //}

            //// завершаем отправку сообщений на сервер
            //await call.RequestStream.CompleteAsync();
            //await readTask;
            #endregion

            #region Получение заголовков
            //// создаем клиент
            //var client = new MessengerHeader.MessengerHeaderClient(channel);

            //// отправляем сообщение серверу
            //using var call = client.SendMessageHeaderAsync(new RequestHeader());

            //// получаем ответ
            //ResponseHeader response = await call.ResponseAsync;
            //Console.WriteLine($"Response: {response.Content}");
            //// получаем все заголовки и выводим их на консоль
            //var headers = await call.ResponseHeadersAsync;
            //foreach (var header in headers)
            //{
            //    Console.WriteLine($"{header.Key}: {header.Value}");
            //}
            #endregion

            #region CRUD
            // создаем клиент
            //var client = new UserService.UserServiceClient(channel);

            //// получение списка объектов
            //ListReply users = await client.ListUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());

            //foreach (var user in users.Users)
            //{
            //    Console.WriteLine($"{user.Id}. {user.Name} - {user.Age}");
            //}

            //try
            //{
            //    // получение одного объекта по id = 2
            //    UserReply user = await client.GetUserAsync(new GetUserRequest { Id = 2 });
            //    Console.WriteLine($"{user.Id}. {user.Name} - {user.Age}");
            //}
            //catch (RpcException ex)
            //{
            //    Console.WriteLine(ex.Status.Detail);    // получаем статус ответа
            //}

            //// добавление одного объекта
            //UserReply user = await client.CreateUserAsync(new CreateUserRequest { Name = "Alice", Age = 32 });
            //Console.WriteLine($"{user.Id}. {user.Name} - {user.Age}");


            //try
            //{
            //    //обновление одного объекта - изменим имя у объекта с id = 1 на Tomas
            //    UserReply user = await client.UpdateUserAsync(new UpdateUserRequest { Id = 1, Name = "Tomas", Age = 38 });
            //    Console.WriteLine($"{user.Id}. {user.Name} - {user.Age}");
            //}
            //catch (RpcException ex)
            //{
            //    Console.WriteLine(ex.Status.Detail);
            //}

            //try
            //{
            //    // удаление объекта с id = 2
            //    UserReply user = await client.DeleteUserAsync(new DeleteUserRequest { Id = 2 });
            //    Console.WriteLine($"{user.Id}. {user.Name} - {user.Age}");
            //}
            //catch (RpcException ex)
            //{
            //    Console.WriteLine(ex.Status.Detail);
            //}
            #endregion

            Console.ReadKey();
        }
    }
}
