using System;
using System.Threading.Tasks;

namespace WebSocketStreamer.Networking
{
    public class WebSocketStreamerReconnection
    {
        private readonly WebSocketStreamerClient _client;
        private readonly int _maxRetries;
        private readonly int _baseDelayMs;

        public WebSocketStreamerReconnection(WebSocketStreamerClient client, int maxRetries = 5, int baseDelayMs = 2000)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            _client = client;
            _maxRetries = maxRetries;
            _baseDelayMs = baseDelayMs;

            _client.Closed += OnClosed;
        }

        private async void OnClosed(ushort code, string reason)
        {
            await AttemptReconnect();
        }

        private async Task AttemptReconnect()
        {
            int retries = 0;

            while (!_client.IsConnected && retries < _maxRetries)
            {
                retries++;
                try
                {
                    await Task.Delay(_baseDelayMs * retries);
                    await _client.Connect();
                }
                catch
                {
                    // Ignore errors and keep trying to reconnect!
                }
            }
        }
    }
}