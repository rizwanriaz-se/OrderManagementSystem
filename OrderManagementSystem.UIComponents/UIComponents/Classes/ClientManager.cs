using DevExpress.Pdf.Native.BouncyCastle.Asn1.Ocsp;
using DevExpress.XtraRichEdit.Fields.Expression;
//using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
//using OrderManagementSystem.Cache.Models;
using System.Text.Json;
using System.Diagnostics;
//using OrderManagementSystem.Cache;
using System.Collections.ObjectModel;
using OrderManagementSystem.Repositories.Repositories;

//using System.ServiceModel.Channels;

namespace OrderManagementSystem.UIComponents.UIComponents.Classes
{
    public class ClientManager
    {
        private System.Timers.Timer _heartbeatTimer;
        private TcpClient _client;

        public TcpClient Client
        {
            get { return _client; }
        }

        private NetworkStream _stream;
        //public NetworkStream Stream
        //{
        //    get { return _stream; }
        //}

        public bool Connected = false;

        private static ClientManager m_Instance;

        public event Action onConnected;
        private ClientManager() { }

        public static ClientManager Instance()
        {
            if (m_Instance == null) m_Instance = new ClientManager();
            return m_Instance;
        }

        public async Task ConnectToServer()
        {
            // Connect to server
            try
            {
                _client = new TcpClient("127.0.0.1", 4444);
                if (_client.Connected)
                {
                    Connected = true;

                    // Moving it up here as it was causing a null reference exception
                    // As the client gets connected, the onConnected is invoked which starts AuthWindow, that writes data to stream for authentication, listen for server response etc so it should be initialized before that.
                    _stream = _client.GetStream();

                    InitializeHeartbeat();
                    ListenAsync();

                    _heartbeatTimer.Start();

                    onConnected?.Invoke();
                }

                //_stream = _client.GetStream();

                //InitializeHeartbeat();
                //ListenAsync();

                //_heartbeatTimer.Start();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Connection error: {ex.Message}");
            }
        }

        private async Task ListenAsync()
        {
            string messageBuffer = string.Empty;
            Debug.WriteLine("START LISTEN");
            try
            {
                while (Connected)
                {
                    byte[] buffer = new byte[25600];
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

                    messageBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    while (TryExtractJson(ref messageBuffer, out string jsonMessage))
                    {
                        Debug.WriteLine("Received on client: " + jsonMessage);
                        ProcessResponse(jsonMessage);
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
                Console.WriteLine($"Error extracting JSON: {ex.Message}");
            }

            return false; // No valid JSON found yet
        }

        private void ProcessResponse(string response)
        {
            try
            {
                Classes.Response responseObject = JsonSerializer.Deserialize<Classes.Response>(response);

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
                //GUIHandler.GetInstance().MessageProcessor.ReceiveMessage(resp.MessageType, resp.MessageAction, resp.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing response: {ex.Message}");
            }
        }

        public async Task InitializeHeartbeat()
        {
            _heartbeatTimer = new System.Timers.Timer(5000); // 5 seconds interval
            _heartbeatTimer.Elapsed += async (sender, e) => await SendHeartbeat();
            _heartbeatTimer.AutoReset = true;
        }


        private async Task SendHeartbeat()
        {
            if (_client != null && _client.Connected)
            {
                try
                {

                    GUIHandler.GetInstance().MessageProcessor.SendMessage(Enums.MessageType.Heartbeat, Enums.MessageAction.Ping, "PING");
                }
                catch
                {
                    HandleServerDisconnected();
                }
            }
        }

        private void HandleServerDisconnected()
        {
            Console.WriteLine("Server disconnected.");
            Connected = false;
            _heartbeatTimer.Stop();
        }


        public async Task SendMessage(Classes.Request request)
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
            }
        }

        //public void ReceiveMessage(Classes.Response response)
        //{
        //    Debug.WriteLine($"Data Received on client: {response.Data}");

        //    // Deserialize response.Data to Category type
        //    if (response != null)
        //    {
        //        switch (response.MessageType)
        //        {
        //            case Enums.MessageType.Category:
        //                switch (response.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        GUIHandler.GetInstance().CacheManager.AddCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Delete:

        //                        GUIHandler.GetInstance().CacheManager.DeleteCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Update:
        //                        GUIHandler.GetInstance().CacheManager.UpdateCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Load:
        //                        ObservableCollection<Category> categories = JsonSerializer.Deserialize<ObservableCollection<Category>>(response.Data.ToString());
        //                        GUIHandler.GetInstance().CacheManager.LoadCategories(categories);
        //                        break;
        //                }
        //                break;
        //            case Enums.MessageType.Product:
        //                switch (response.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        GUIHandler.GetInstance().CacheManager.AddProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Delete:
        //                        GUIHandler.GetInstance().CacheManager.DeleteProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Update:
        //                        GUIHandler.GetInstance().CacheManager.UpdateProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Load:
        //                        ObservableCollection<Product> products = JsonSerializer.Deserialize<ObservableCollection<Product>>(response.Data.ToString());
        //                        GUIHandler.GetInstance().CacheManager.LoadProducts(products);
        //                        break;
        //                }
        //                break;
        //            case Enums.MessageType.Order:
        //                switch (response.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        GUIHandler.GetInstance().CacheManager.AddOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Delete:
        //                        GUIHandler.GetInstance().CacheManager.DeleteOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Update:
        //                        GUIHandler.GetInstance().CacheManager.UpdateOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Load:
        //                        ObservableCollection<Order> orders = JsonSerializer.Deserialize<ObservableCollection<Order>>(response.Data.ToString());
        //                        GUIHandler.GetInstance().CacheManager.LoadOrders(orders);
        //                        break;
        //                }
        //                break;
        //            case Enums.MessageType.User:
        //                switch (response.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        GUIHandler.GetInstance().CacheManager.AddUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Delete:
        //                        GUIHandler.GetInstance().CacheManager.DeleteUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Update:
        //                        GUIHandler.GetInstance().CacheManager.UpdateUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Load:
        //                        ObservableCollection<User> users = JsonSerializer.Deserialize<ObservableCollection<User>>(response.Data.ToString());
        //                        GUIHandler.GetInstance().CacheManager.LoadUsers(users);
        //                        break;
        //                }
        //                break;
        //            default:
        //                Debug.WriteLine("Unhandled message type.");
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        Debug.WriteLine("Failed to deserialize response.Data to Category.");
        //    }
        //}
    }
}