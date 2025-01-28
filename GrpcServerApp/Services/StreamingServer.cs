using Google.Protobuf;
using Grpc.Core;
using ServerStream;

namespace GrpcServerApp.Services
{
    public class StreamingServer: MessengerServerStream.MessengerServerStreamBase
    {
        private readonly ILogger<StreamingServer> _logger;
        private readonly HostModel _hostModel;

        public StreamingServer(ILogger<StreamingServer> logger, HostModel hostModel)
        {
            _logger = logger;
            _hostModel = hostModel;
        }
        public async override Task StreamingFromServer(RequestServerStream request, IServerStreamWriter<ResponseServerStream> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new ResponseServerStream { Content = _hostModel.Text });
                // для имитации работы делаем задержку в 3 секунду
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
           
        }
    }
}
