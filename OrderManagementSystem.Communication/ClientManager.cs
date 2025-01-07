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
        private static ClientManager m_objInstance;
        private System.Timers.Timer m_objHeartbeatTimer;
        private System.Timers.Timer m_objRetryTimer;
        private TcpClient m_objClient;
        private CancellationToken m_objToken;
        private CancellationTokenSource m_objCts;
        private NetworkStream m_objStream;

        public TcpClient Client
        {
            get { return m_objClient; }
        }
        public bool Connected = false;
        public event Action OnConnected;
        public event Action<string> OnReceive;
        public event Action OnDisconnected;

        private ClientManager()
        {
            m_objClient = new TcpClient();
            m_objHeartbeatTimer = new System.Timers.Timer(Constants.HeartbeatInterval);
            m_objRetryTimer = new System.Timers.Timer(5000);
        }

        public static ClientManager Instance
        {
            get { 
                if (m_objInstance == null) 
                    m_objInstance = new ClientManager();
                return m_objInstance;
            }
        }

        public void ConnectToServer()
        {
            Random RandomInt = new Random();
            string RandomPort = $"444{RandomInt.Next(1, 10)}";

            try
            {
                m_objClient = new TcpClient(new System.Net.IPEndPoint(System.Net.IPAddress.Any, Int32.Parse(RandomPort)));
                m_objClient.Connect(Constants.IPAddress, Constants.Port);
                if (m_objClient.Connected)
                {
                    Connected = true;
                    m_objStream = Client.GetStream();
                    OnConnected?.Invoke();
                    m_objHeartbeatTimer.Start();
                }
            }
            catch (SocketException ex)
            {
                Debug.WriteLine($"Socket error: {ex.Message}");
                OnDisconnected?.Invoke();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Connection error: {ex.Message}");
            }
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
                    m_objRetryTimer.Elapsed -= RetryTimerElapsed; 
                    m_objRetryTimer.Elapsed += RetryTimerElapsed; 
                    m_objRetryTimer.AutoReset = true;
                    m_objRetryTimer.Start();
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
            m_objHeartbeatTimer.Stop();

            m_objStream?.Close();
            m_objClient?.Close();

        }

        private async void RetryTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_objRetryTimer.Stop(); // Stop the timer to prevent multiple concurrent attempts
            ConnectToServer();
            if (!Connected)
            {
                m_objRetryTimer.Start(); // Restart the timer if the connection attempt failed
            }
        }

        public void ListenAsync()
        {
            string messageBuffer = string.Empty;
            Debug.WriteLine("START LISTEN");
            try
            {
                while (Connected)
                {
                    byte[] buffer = new byte[Constants.BufferSize];
                    int bytesRead = m_objStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
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

                if (responseObject.Error != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                        DXMessageBox.Show(responseObject.Error,
                                               "Error",
                                               System.Windows.MessageBoxButton.OK
                                    ));
                    return;
                }
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing response: {ex.Message}");
            }
        }

        public async Task InitializeHeartbeat()
        {
            m_objHeartbeatTimer.Elapsed += async (sender, e) => await SendHeartbeat();
            m_objHeartbeatTimer.AutoReset = true;
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
            }
        }

       
        public async Task SendMessage(Request request)
        {
            try
            {
                string json = JsonSerializer.Serialize(request);
                byte[] data = Encoding.UTF8.GetBytes(json);
                await m_objStream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in SendMessage: " + ex.Message);
                OnDisconnected?.Invoke();
            }
        }
    }
}