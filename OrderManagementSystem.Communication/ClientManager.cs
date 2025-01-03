using DevExpress.Xpf.Core;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace OrderManagementSystem.UIComponents.Classes
{
    public class ClientManager
    {
        private static ClientManager m_Instance;
        private System.Timers.Timer _heartbeatTimer;
        private System.Timers.Timer _retryTimer;
        private TcpClient _client;
        private CancellationToken _token;
        private CancellationTokenSource _cts;
        private NetworkStream _stream;
        //public LingerOption lingerOption;


        public TcpClient Client
        {
            get { return _client; }
        }
        public bool Connected = false;
        public event Action OnConnected;
        public event Action<string> OnReceive;
        public event Action OnDisconnected;

        private ClientManager()
        {

            _client = new TcpClient();
            _heartbeatTimer = new System.Timers.Timer(Constants.HeartbeatInterval);
            _retryTimer = new System.Timers.Timer(5000);
        }

        public static ClientManager Instance()
        {
            if (m_Instance == null) m_Instance = new ClientManager();
            return m_Instance;
        }

        public void ConnectToServer()
        {
            Random RandomInt = new Random();
            string RandomPort = $"444{RandomInt.Next(1, 10)}";

            //while (!Connected)
            //{
            try
            {
               
                _client = new TcpClient(new System.Net.IPEndPoint(System.Net.IPAddress.Any, Int32.Parse(RandomPort)));
                _client.Connect(Constants.IPAddress, Constants.Port);
                if (_client.Connected)
                {
                    Connected = true;
                    _stream = Client.GetStream();
                    OnConnected?.Invoke();
                    _heartbeatTimer.Start();
                }
                //}
                //else
                //{
                //Connected = false;
                //Client.Close();
                //OnDisconnected?.Invoke();
                //_heartbeatTimer.Stop();
                //}

            }
            catch (SocketException ex)
            {
                Debug.WriteLine($"Socket error: {ex.Message}");
                OnDisconnected?.Invoke();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Connection error: {ex.Message}");
                //OnDisconnected?.Invoke();

                //                MessageBoxResult result = System.Windows.Application.Current.Dispatcher.Invoke(()=> DXMessageBox.Show(
                //$"Failed to connect to server. {ex.Message}. Do you want to try again?",
                //               "Error",
                //               System.Windows.MessageBoxButton.YesNo
                //               ));
                //if (result == MessageBoxResult.Yes)
                //{
                //    ConnectToServer();
                //}
                //else
                //{
                //}
                //if (result == System.Windows.MessageBoxResult.No)
                //{
                //    HandleDisconnection();
                //    return;
                //    //if (System.Windows.Application.Current != null)
                //    //{
                //    //    //HandleServerDisconnected();
                //    //    System.Windows.Application.Current.Dispatcher.Invoke(() => System.Windows.Application.Current.Shutdown());
                //    //}
                //}
                //Debug.WriteLine("Retrying connection...");
                //await Task.Delay(2000); // Wait before retrying
            }

            //if (!Connected)
            //{
            //    Task.Delay(Constants.HeartbeatTimeout); // Wait before retrying

            //}
            //}
        }

        public void HandleDisconnection()
        {
            Connected = false;
            if (System.Windows.Application.Current != null)
            {
                MessageBoxResult result = System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    DXMessageBox.Show($"Failed to connect to server. Do you want to try again?",
                           "Error",
                           System.Windows.MessageBoxButton.YesNo
                ));
                if (result == MessageBoxResult.Yes)
                {
                    _retryTimer.Elapsed -= RetryTimerElapsed; 
                    _retryTimer.Elapsed += RetryTimerElapsed; 
                    _retryTimer.AutoReset = true;
                    _retryTimer.Start();
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => System.Windows.Application.Current.Shutdown());
                }
            }
            else
            {
                DXMessageBox.Show($"Failed to connect to server.",
                           "Error",
                           System.Windows.MessageBoxButton.OK
                );
            }
            _heartbeatTimer.Stop();

            _stream?.Close();
            _client?.Close();

            //OnDisconnected?.Invoke();
        }

        private async void RetryTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _retryTimer.Stop(); // Stop the timer to prevent multiple concurrent attempts
            ConnectToServer();
            if (!Connected)
            {
                _retryTimer.Start(); // Restart the timer if the connection attempt failed
            }
        }

        //public void HandleDisconnection()
        //{
        //    Connected = false;
        //    if (System.Windows.Application.Current != null)
        //    {
        //        MessageBoxResult result = System.Windows.Application.Current.Dispatcher.Invoke(() =>
        //            DXMessageBox.Show($"Failed to connect to server. Do you want to try again?",
        //                   "Error",
        //                   System.Windows.MessageBoxButton.YesNo
        //            ));
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            _retryTimer.Start();
        //            //System.Windows.Application.Current.Dispatcher.Invoke(() =>
        //            //DXMessageBox.Show($"Retrying in {TimeSpan.FromSeconds(5)}",
        //            //       "Info",
        //            //       System.Windows.MessageBoxButton.OK
        //            //));
        //            _retryTimer.Elapsed += async (sender, e) => ConnectToServer();
        //            _retryTimer.Stop();
        //            //_retryTimer.AutoReset = true;
        //        }
        //        else
        //        {
        //            System.Windows.Application.Current.Dispatcher.Invoke(() => System.Windows.Application.Current.Shutdown());
        //        }

        //    }
        //    else
        //    {
        //        DXMessageBox.Show($"Failed to connect to server.",
        //                   "Error",
        //                   System.Windows.MessageBoxButton.OK
        //            );
        //    }
        //    _heartbeatTimer.Stop();

        //    _stream?.Close();
        //    _client?.Close();

        //    //OnDisconnected?.Invoke();
        //}
        //    public void ConnectToServer()
        //    {
        //        LingerOption lingerOption = new LingerOption(true, 0);

        //        while (!Connected)
        //        {
        //            try
        //            {
        //                //Client.LingerState = lingerOption;

        //                Client.Connect(Constants.IPAddress, Constants.Port);

        //                if (Client.Connected)
        //                {
        //                    Connected = true;
        //                    _stream = Client.GetStream();
        //                    OnConnected?.Invoke();
        //                    _heartbeatTimer.Start();
        //                }
        //                //else
        //                //{
        //                //    Connected = false;
        //                //    Client.Close();
        //                //    OnDisconnected?.Invoke();
        //                //    _heartbeatTimer.Stop();
        //                //}
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine($"Connection error: {ex.Message}");

        //                MessageBoxResult result = DXMessageBox.Show(
        //$"Failed to connect to server. {ex.Message}. Do you want to try again?",
        //               "Error",
        //               System.Windows.MessageBoxButton.YesNo
        //               );
        //                if (result == MessageBoxResult.Yes)
        //                {
        //                    ConnectToServer();
        //                }
        //                //else
        //                //{
        //                //}
        //                if (result == System.Windows.MessageBoxResult.No)
        //                {
        //                    if (System.Windows.Application.Current != null)
        //                    {
        //                        //HandleServerDisconnected();
        //                        System.Windows.Application.Current.Dispatcher.Invoke(() => System.Windows.Application.Current.Shutdown());
        //                    }
        //                }
        //            }

        //            if (!Connected)
        //            {
        //                Task.Delay(Constants.HeartbeatTimeout); // Wait before retrying

        //            }
        //        }
        //    }


        public void ListenAsync()
        {
            string messageBuffer = string.Empty;
            Debug.WriteLine("START LISTEN");
            try
            {
                while (Connected)
                {
                    byte[] buffer = new byte[Constants.BufferSize];
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        //HandleDisconnection();
                        return;
                    }

                    messageBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    while (TryExtractJson(ref messageBuffer, out string jsonMessage))
                    {
                        Debug.WriteLine("Received on client: " + jsonMessage);
                        OnReceive?.Invoke(jsonMessage);

                    }
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.WriteLine($"Received on client: {response}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in listener: " + ex.Message);
                //HandleDisconnection();
            }
        }

        private static bool TryExtractJson(ref string buffer, out string json)
        {
            json = null;

            try
            {
                int openBraces = 0, closeBraces = 0;

                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == '{') openBraces++;
                    if (buffer[i] == '}') closeBraces++;

                    // When we find matching braces
                    if (openBraces > 0 && openBraces == closeBraces)
                    {
                        json = buffer.Substring(0, i + 1);
                        buffer = buffer.Substring(i + 1);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error extracting JSON: {ex.Message}");
            }

            return false; // No valid JSON found yet
        }

        public void ProcessResponse(string response)
        {
            try
            {
                Response responseObject = JsonSerializer.Deserialize<Response>(response);


                //if (responseObject.MessageType == Enums.MessageType.Heartbeat && responseObject.MessageAction == Enums.MessageAction.Ping)
                //{
                //    m_bPingResponseReceived = true;
                //    return;
                //}

                if (responseObject.Error == null) { 
                    switch (responseObject.MessageType)
                {
                    case Enums.MessageType.Category:
                        MessageProcessor.ProcessCategoryMessage(responseObject);
                        break;
                    case Enums.MessageType.Order:
                        MessageProcessor.ProcessOrderMessage(responseObject);
                        break;
                    case Enums.MessageType.Product:
                        MessageProcessor.ProcessProductMessage(responseObject);
                        break;
                    case Enums.MessageType.User:
                        MessageProcessor.ProcessUserMessage(responseObject);
                        break;
                    case Enums.MessageType.Error:
                        break;
                    default:
                        break;
                }
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        DXMessageBox.Show(responseObject.Error,
                                               "Error",
                                               System.Windows.MessageBoxButton.OK
                                    ));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing response: {ex.Message}");
            }
        }

        public async Task InitializeHeartbeat()
        {
            _heartbeatTimer.Elapsed += async (sender, e) => await SendHeartbeat();
            _heartbeatTimer.AutoReset = true;
        }

        private async Task SendHeartbeat()
        {
            try
            {
                MessageProcessor.SendMessage(Enums.MessageType.Heartbeat, Enums.MessageAction.Ping, "PING");
            }
            catch (Exception ex)
            {

                // Without Dispatcher in case of MessageBox, got error: System.InvalidOperationException: 'The calling thread must be STA, because many UI components require this.'

                Debug.WriteLine("Error in SendHeartbeat: " + ex.Message);
                //HandleDisconnection();
            }
        }

        //public void HandleServerDisconnected()
        //{
        //    Debug.WriteLine("Server disconnected.");
        //    Connected = false;
        //    Client.Close();

        //    if (_heartbeatTimer != null)
        //    {
        //        _heartbeatTimer.Stop();
        //    }
        //    if (System.Windows.Application.Current != null)
        //    {
        //        MessageBoxResult result = System.Windows.Application.Current.Dispatcher.Invoke(() => DXMessageBox.Show("Failed to connect to server. Do you want to try again?", "Error", MessageBoxButton.YesNo));

        //        if (result == MessageBoxResult.No)
        //        {
        //            System.Windows.Application.Current.Dispatcher.Invoke(() => System.Windows.Application.Current.Shutdown());
        //        }
        //        if (_heartbeatTimer != null)
        //        {
        //            _heartbeatTimer.Stop();
        //        }
        //    }
        //}
        public async Task SendMessage(Request request)
        {
            try
            {
                string json = JsonSerializer.Serialize(request);
                byte[] data = Encoding.UTF8.GetBytes(json);
                await _stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in SendMessage: " + ex.Message);
                //HandleServerDisconnected();
                OnDisconnected?.Invoke();
            }
        }
    }
}