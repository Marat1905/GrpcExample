using Grpc.Core;

namespace GrpcServiceApp.Services
{
    public class MessengerClientStreamService:MessengerClientStream.MessengerClientStreamBase
    {
        public async override Task<ResponseClientStream> StreamingFromClient(IAsyncStreamReader<RequestClientStream> requestStream, ServerCallContext context)
        {
            await foreach (RequestClientStream request in requestStream.ReadAllAsync())
            {
                Console.WriteLine(request.Content);
            }
            Console.WriteLine("Все данные получены...");
            return new ResponseClientStream { Content = "все данные получены" };
        }
    }
}
