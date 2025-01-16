using DevExpress.Xpf.Core;
using OrderManagementSystem.Cache;
using OrderManagementSystemServer.Repository;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
//using Timer = System.Timers.Timer;

namespace OrderManagementSystem.UIComponents.Classes
{
    public class Request
    {
        public MessageType MessageType { get; set; }
        public MessageAction MessageAction { get; set; }
        public object? Data { get; set; }
    }

    public class Response
    {
        public MessageType MessageType { get; set; }
        public MessageAction MessageAction { get; set; }
        public object? Data { get; set; }
        public string? Error { get; set; }
    }

    public enum MessageType
    {
        Category,
        Order,
        Product,
        User,
        Heartbeat,
        Error
    }

    public enum MessageAction
    {
        Add,
        Update,
        Delete,
        Load,
        Ping,
        Error
    }

    public class ClientManager
    {
        private static ClientManager? m_objInstance;
        private System.Timers.Timer m_objHeartbeatTimer;
        private System.Timers.Timer m_objRetryTimer;
        private TcpClient m_objClient;
        private NetworkStream m_objStream;
        private bool m_IsDataLoaded = false;


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
            get
            {
                if (m_objInstance == null)
                    m_objInstance = new ClientManager();
                return m_objInstance;
            }
        }

        public bool IsDataLoaded
        {
            get { return m_IsDataLoaded; }
            set { m_IsDataLoaded = value; }
        }

        public void ConnectToServer()
        {
            Random RandomInt = new Random();
            string RandomPort = $"444{RandomInt.Next(1, 10)}";

            try
            {
                m_objClient = new TcpClient(new IPEndPoint(IPAddress.Any, Int32.Parse(RandomPort)));
                m_objClient.Connect(Constants.IPAddress, Constants.Port);
                if (m_objClient.Connected)
                {
                    Connected = true;
                    if (CacheManager.Instance.Products.Count != 0 && CacheManager.Instance.Orders.Count !=0 && CacheManager.Instance.Categories.Count != 0)
                    {

                        IsDataLoaded = true;
                    }
                    m_objStream = Client.GetStream();
                    OnConnected?.Invoke();
                    m_objHeartbeatTimer.Start();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                OnDisconnected?.Invoke();
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
                           MessageBoxButton.OK
                );
            }
            m_objHeartbeatTimer.Stop();

            m_objStream?.Close();
            m_objClient?.Close();

        }

        private async void RetryTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_objRetryTimer.Stop();
            ConnectToServer();
            if (!Connected)
            {
                m_objRetryTimer.Start();
            }
        }

        public void LoadData()
        {
            //SendMessage(MessageType.User, MessageAction.Load, null);
            SendMessage(MessageType.Category, MessageAction.Load, null);
            SendMessage(MessageType.Product, MessageAction.Load, null);
            SendMessage(MessageType.Order, MessageAction.Load, null);

            Thread.Sleep(2000);

            if (CacheManager.Instance.Products != null && CacheManager.Instance.Orders != null && CacheManager.Instance.Categories != null)
            {
                IsDataLoaded = true;
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
                        OnReceive?.Invoke(jsonMessage);
                    }
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in listener: " + ex.Message);
                System.Windows.Application.Current.Dispatcher.Invoke(()=> DXMessageBox.Show("Error when trying to read data from server.", "Error"));
                return;
            }
        }

        private static bool TryExtractJson(ref string buffer, out string json)
        {
            json = null;


            int openBraces = 0, closeBraces = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == '{') openBraces++;
                if (buffer[i] == '}') closeBraces++;

                if (openBraces > 0 && openBraces == closeBraces)
                {
                    json = buffer.Substring(0, i + 1);
                    buffer = buffer.Substring(i + 1);
                    return true;
                }
            }

            return false;
        }

        public void ProcessResponse(string response)
        {
            Response? responseObject = DeserializeJson<Response>(response);

            if (responseObject == null)
            {
                DXMessageBox.Show("Some unexpected error occurred when trying to process server response. Please try again.");
                return;
            }

            if (responseObject.Data == null)
            {
                DXMessageBox.Show("Error trying to process server response. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (responseObject?.Error != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    DXMessageBox.Show(responseObject.Error,
                                           "Error",
                                           MessageBoxButton.OK
                                ));
                return;
            }
            switch (responseObject?.MessageType)
            {
                case MessageType.Category:
                    ProcessCategoryMessage(responseObject);
                    break;
                case MessageType.Order:
                    ProcessOrderMessage(responseObject);
                    break;
                case MessageType.Product:
                    ProcessProductMessage(responseObject);
                    break;
                case MessageType.User:
                    ProcessUserMessage(responseObject);
                    break;
                default:
                    break;
            }
        }

        private string? SerializeJson(Request request)
        {
            try
            {
                if (request != null)
                    return JsonSerializer.Serialize(request);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unexpected error occurred when trying to serialize data.");
                return null;
            }
        }

        private T? DeserializeJson<T>(object responseData)
        {
            try
            {
                if (responseData != null)
                {
                    string data = responseData.ToString();
                    return (T)JsonSerializer.Deserialize<T>(data);
                }
                return default;
            }
            catch (Exception)
            {
                DXMessageBox.Show("Error trying to process server response. Please try again.");
                Debug.WriteLine("Unexpected error occurred when trying to deserialize data.");
                return default;
            }
        }

        public void ProcessCategoryMessage(Response response)
        {
            switch (response.MessageAction)
                {
                    case MessageAction.Add:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.AddCategory(DeserializeJson<Category>(response.Data)));
                        break;
                    case MessageAction.Update:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.UpdateCategory(DeserializeJson<Category>(response.Data)));
                        break;
                    case MessageAction.Delete:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.DeleteCategory(response.Data.ToString()));
                        break;
                    case MessageAction.Load:
                        ObservableCollection<Category> categories = DeserializeJson<ObservableCollection<Category>>(response.Data);
                        CacheManager.Instance.Categories = categories;
                        //CacheManager.Instance.LoadCategories(categories);
                        break;
                    default:
                        break;
            }
        }

        public void ProcessOrderMessage(Response response)
        {
            try
            {
                switch (response.MessageAction)
                {
                    case MessageAction.Add:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.AddOrder(DeserializeJson<Order>(response.Data)));
                        break;
                    case MessageAction.Delete:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.DeleteOrder(response.Data.ToString()));
                        break;
                    case MessageAction.Update:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.UpdateOrder(DeserializeJson<Order>(response.Data)));
                        break;
                    case MessageAction.Load:
                        if (response.Data == null)
                        {
                            DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        ObservableCollection<Order> orders = DeserializeJson<ObservableCollection<Order>>(response.Data);
                        CacheManager.Instance.Orders = orders;
                        //CacheManager.Instance.LoadOrders(orders);
                        break;
                };
            }
            catch (Exception ex)
            {
                DXMessageBox.Show($"Error trying to {response.MessageAction} {response.MessageType}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }

        public void ProcessProductMessage(Response response)
        {
            try
            {
                switch (response.MessageAction)
                {
                    case MessageAction.Add:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.AddProduct(DeserializeJson<Product>(response.Data)));
                        break;
                    case MessageAction.Delete:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.DeleteProduct(response.Data.ToString()));
                        break;
                    case MessageAction.Update:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.UpdateProduct(DeserializeJson<Product>(response.Data)));
                        break;
                    case MessageAction.Load:
                        if (response.Data == null)
                        {
                            DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            return;
                        }
                        ObservableCollection<Product> products = DeserializeJson<ObservableCollection<Product>>(response.Data);
                        CacheManager.Instance.Products = products;
                        //CacheManager.Instance.LoadProducts(products);
                        break;

                };

            }
            catch (Exception ex)
            {
                DXMessageBox.Show($"Error trying to {response.MessageAction} {response.MessageType}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
        }

        public void ProcessUserMessage(Response response)
        {
            try
            {

                switch (response.MessageAction)
                {
                    case MessageAction.Add:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.AddUser(DeserializeJson<User>(response.Data)));
                        break;
                    case MessageAction.Delete:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.DeleteUser(response.Data.ToString()));
                        break;
                    case MessageAction.Update:
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.UpdateUser(DeserializeJson<User>(response.Data)));
                        break;
                    case MessageAction.Load:
                        if (response.Data == null)
                        {
                            //throw new Exception("Error trying to load data from server. Please try again.");
                            DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        ObservableCollection<User> users = DeserializeJson<ObservableCollection<User>>(response.Data);
                        CacheManager.Instance.Users = users;
                        //CacheManager.Instance.LoadUsers(users);
                        break;

                };
            }

            catch (Exception ex)
            {
                DXMessageBox.Show($"Error trying to {response.MessageAction} {response.MessageType}", "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public async Task InitializeHeartbeat()
        {
            m_objHeartbeatTimer.Elapsed += async (sender, e) =>
            SendMessage(MessageType.Heartbeat, MessageAction.Ping, "PING");
            m_objHeartbeatTimer.AutoReset = true;
        }


        public async void SendMessage(MessageType messageType, MessageAction messageAction, object? message = null)
        {


            try
            {
                string? json = SerializeJson(new Request
                {
                    MessageAction = messageAction,
                    MessageType = messageType,
                    Data = message
                });
                if (json == null)
                {
                    DXMessageBox.Show("Some unexpected error occurred. Please try again.");
                    return;
                }
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