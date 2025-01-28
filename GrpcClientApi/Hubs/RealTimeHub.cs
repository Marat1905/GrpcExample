using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace GrpcClientApi.Hubs
{
    public class RealTimeHub:Hub
    {
        public RealTimeHub()
        {
            
        }
        public async IAsyncEnumerable<string> TriggerStream([EnumeratorCancellation]CancellationToken cancellationToken)
        {
            int i = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
               
                yield return $"Job {i++} executed successfully";
                await Task.Delay(1000, cancellationToken);
            }

           
        }
    }
}
