using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace EWiki.XF.Service
{
    public interface ISniperHubService
    {
        Task Connect();
        Task Send(string message);
        event EventHandler<string> OnMessageReceived;
    }

    public class SniperHubService : ISniperHubService
    {
        private readonly HubConnection _connection;
        private readonly IHubProxy _proxy;

        public event EventHandler<string> OnMessageReceived;

        public SniperHubService()
        {
            _connection = new HubConnection("http://pokitapp.azurewebsites.net/");
            _proxy = _connection.CreateHubProxy("SniperHub");
        }

        #region ISniperHubService implementation

        public async Task Connect()
        {
            try
            {
                await _connection.Start();
            }
            catch (Exception exception)
            {
                throw;
            }

            _proxy.On("GetMessage", (string message) => OnMessageReceived(this, message));
        }

        public async Task Send(string message)
        {
            await _proxy.Invoke("SendMessage", message);
        }

        #endregion
    }
}
