using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using ServerStream;
using System.Runtime.CompilerServices;

namespace GrpcClientApi.Hubs
{
    public class RealTimeHub:Hub
    {
        private readonly MessengerServerStream.MessengerServerStreamClient _client;
        private readonly ILogger<RealTimeHub> _logger;
        private AsyncServerStreamingCall<ResponseServerStream> _call;

        public RealTimeHub(ILogger<RealTimeHub> logger, MessengerServerStream.MessengerServerStreamClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Send", message);
        }

        public async IAsyncEnumerable<string> Counter(
        int delay,
        [EnumeratorCancellation]
        CancellationToken cancellationToken)
        {

            var serverData = _client.StreamingFromServer(new RequestServerStream());

            // получаем поток сервера
            var responseStream = serverData.ResponseStream;
            // с помощью итераторов извлекаем каждое сообщение из потока
            while (await responseStream.MoveNext(cancellationToken))
            {

                ResponseServerStream response = responseStream.Current;
                yield return $"{response.Content}";
                Console.WriteLine(response.Content);
            }
        }
    }
}
