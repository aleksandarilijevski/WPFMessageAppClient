using Prism.Commands;
using Prism.Mvvm;
using System.Net;
using System.Net.Sockets;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;

namespace WPFMessageAppClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _chat;
        private string _messageToSend;
        private Socket _socket;
        private DelegateCommand _sendMessageCommand;
        private DelegateCommand _connectToServerCommand;

        public string MessageToSend
        {
            get
            {
                return _messageToSend;
            }

            set
            {
                _messageToSend = value;
                RaisePropertyChanged();
            }
        }

        public string Chat
        {
            get
            {
                return _chat;
            }

            set
            {
                _chat = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ConnectToServerCommand
        {
            get
            {
                _connectToServerCommand = new DelegateCommand(ConnectToSocket);
                return _connectToServerCommand;
            }
        }

        public DelegateCommand SendMessageCommand
        {
            get
            {
                _sendMessageCommand = new DelegateCommand(SendMessages);
                return _sendMessageCommand;
            }
        }

        /// <summary>
        /// Connecting to server
        /// </summary>
        private async void ConnectToSocket()
        {
            IPAddress ipAddress = IPAddress.Parse("192.168.1.2");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 4321);

            _socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await _socket.ConnectAsync(ipEndPoint);

            await ReceiveMessages(_socket);
        }

        /// <summary>
        /// Receiving message from server
        /// </summary>
        private async Task ReceiveMessages(Socket connectedClient)
        {
            while (true)
            {
                byte[] buffer = new byte[1024];

                int received = await connectedClient.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);
                Chat += "Client said : " + response + "\n";
                RaisePropertyChanged(nameof(Chat));
            }
        }

        /// <summary>
        /// Sending message to server.
        /// </summary>
        private async void SendMessages()
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(_messageToSend);
            int _ = await _socket.SendAsync(messageBytes, SocketFlags.None);
            Chat += "You said : " + _messageToSend + "\n";
            MessageToSend = string.Empty;
        }
    }
}
