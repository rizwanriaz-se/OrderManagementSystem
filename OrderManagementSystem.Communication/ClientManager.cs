using DevExpress.Xpf.Controls;
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

                    //if (CacheManager.Instance.Products.Count != 0 && CacheManager.Instance.Orders.Count != 0 && CacheManager.Instance.Categories.Count != 0)
                    //{
                    //    IsDataLoaded = true;
                    //}
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
                           MessageBoxButton.YesNo, MessageBoxImage.Error
                ));
                if (result == MessageBoxResult.Yes)
                {

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
                           MessageBoxButton.OK, MessageBoxImage.Error
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
            SendMessage(MessageType.Category, MessageAction.Load, null);
            SendMessage(MessageType.Product, MessageAction.Load, null);
            SendMessage(MessageType.Order, MessageAction.Load, null);


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
                DXMessageBox.Show("Some unexpected error occurred when trying to process server response. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                           MessageBoxButton.OK, MessageBoxImage.Error
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
                string data = responseData.ToString();
                return (T)JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception)
            {
                Debug.WriteLine("Unexpected error occurred when trying to deserialize data.");
                return default;
            }
        }

        public void ProcessCategoryMessage(Response response)
        {
            switch (response.MessageAction)
            {
                case MessageAction.Add:
                    Category? categoryAddData = DeserializeJson<Category>(response.Data);
                    if (categoryAddData == null || categoryAddData is not Category)
                    {
                        DXMessageBox.Show("Error trying to Add Category. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.AddCategory(categoryAddData));
                    break;

                case MessageAction.Update:
                    Category categoryUpdateData = DeserializeJson<Category>(response.Data);
                    if (categoryUpdateData == null || categoryUpdateData is not Category)
                    {
                        DXMessageBox.Show("Error trying to Update Category. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.UpdateCategory(categoryUpdateData));
                    break;

                case MessageAction.Delete:
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.DeleteCategory(response.Data.ToString()));
                    break;

                case MessageAction.Load:
                    ObservableCollection<Category> categories = DeserializeJson<ObservableCollection<Category>>(response.Data);
                    if (categories == null || categories is not ObservableCollection<Category>)
                    {
                        DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    CacheManager.Instance.Categories = categories;
                    break;

                default:
                    break;

            }
        }

        public void ProcessOrderMessage(Response response)
        {

            switch (response.MessageAction)
            {
                case MessageAction.Add:
                    Order? orderAddData = DeserializeJson<Order>(response.Data);
                    if (orderAddData == null || orderAddData is not Order)
                    {
                        DXMessageBox.Show("Error trying to Add Order. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.AddOrder(orderAddData));
                    break;

                case MessageAction.Delete:
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.DeleteOrder(response.Data.ToString()));
                    break;

                case MessageAction.Update:
                    Order orderUpdateData = DeserializeJson<Order>(response.Data);
                    if (orderUpdateData == null || orderUpdateData is not Order)
                    {
                        DXMessageBox.Show("Error trying to Update Order. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.UpdateOrder(orderUpdateData));
                    break;

                case MessageAction.Load:
                    ObservableCollection<Order> orders = DeserializeJson<ObservableCollection<Order>>(response.Data);
                    if (orders == null || orders is not ObservableCollection<Order>)
                    {
                        DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    CacheManager.Instance.Orders = orders;
                    break;
            };

        }

        public void ProcessProductMessage(Response response)
        {

            switch (response.MessageAction)
            {
                case MessageAction.Add:
                    Product? productAddData = DeserializeJson<Product>(response.Data);
                    if (productAddData == null || productAddData is not Product)
                    {
                        DXMessageBox.Show("Error trying to Add Product. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.AddProduct(productAddData));
                    break;

                case MessageAction.Delete:
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.DeleteProduct(response.Data.ToString()));
                    break;

                case MessageAction.Update:
                    Product? productUpdateData = DeserializeJson<Product>(response.Data);
                    if (productUpdateData == null || productUpdateData is not Product)
                    {
                        DXMessageBox.Show("Error trying to Update Product. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.UpdateProduct(productUpdateData));
                    break;

                case MessageAction.Load:
                    ObservableCollection<Product> products = DeserializeJson<ObservableCollection<Product>>(response.Data);
                    if (products == null || products is not ObservableCollection<Product>)
                    {
                        DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    CacheManager.Instance.Products = products;
                    break;

            };


        }

        public void ProcessUserMessage(Response response)
        {

            switch (response.MessageAction)
            {
                case MessageAction.Add:
                    User? userAddData = DeserializeJson<User>(response.Data);
                    if (userAddData == null || userAddData is not User)
                    {
                        DXMessageBox.Show("Error trying to Add User. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.AddUser(userAddData));
                    break;

                case MessageAction.Delete:
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.DeleteUser(response.Data.ToString()));
                    break;

                case MessageAction.Update:
                    User? userUpdateData = DeserializeJson<User>(response.Data);
                    if (userUpdateData == null || userUpdateData is not User)
                    {
                        DXMessageBox.Show("Error trying to Update User. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(() => CacheManager.Instance.UpdateUser(userUpdateData));
                    break;

                case MessageAction.Load:
                    ObservableCollection<User> users = DeserializeJson<ObservableCollection<User>>(response.Data);
                    if (users == null || users is not ObservableCollection<User>)
                    {
                        DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    CacheManager.Instance.Users = users;
                    break;

            };

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
                    DXMessageBox.Show("Some unexpected error occurred. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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