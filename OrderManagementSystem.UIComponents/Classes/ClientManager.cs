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
using DevExpress.Xpf.Core;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using System.Windows;
//using OrderManagementSystem.Repositories.Repositories;


//using System.ServiceModel.Channels;

namespace OrderManagementSystem.UIComponents.Classes
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

        //implement logic for else and also retries in case the server is down and up later. Retry could be like telling user to either send retry or not.
        public async Task ConnectToServer()
        {
            // Connect to server
            try
            {
                _client = new TcpClient(Constants.IPAddress, Constants.Port);
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

                MessageBoxResult result = DXMessageBox.Show(
$"Failed to connect to server. {ex.Message}. Do you want to try again?",
               "Error",
               System.Windows.MessageBoxButton.YesNo
               );

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    ConnectToServer();
                }
                else if (result == System.Windows.MessageBoxResult.No)
                {
                    HandleServerDisconnected();
                }
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
                    byte[] buffer = new byte[Constants.BufferSize];
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
                Debug.WriteLine($"Error extracting JSON: {ex.Message}");
            }

            return false; // No valid JSON found yet
        }

        private void ProcessResponse(string response)
        {
            try
            {
                Response responseObject = JsonSerializer.Deserialize<Response>(response);

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
                //GUIHandler.Instance.MessageProcessor.ReceiveMessage(resp.MessageType, resp.MessageAction, resp.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing response: {ex.Message}");
            }
        }

        public async Task InitializeHeartbeat()
        {
            _heartbeatTimer = new System.Timers.Timer(Constants.HeartbeatInterval); // 5 seconds interval
            _heartbeatTimer.Elapsed += async (sender, e) => await SendHeartbeat();
            _heartbeatTimer.AutoReset = true;
        }

        private async Task SendHeartbeat()
        {

            try
            {
                if (_client != null && _client.Connected)

                    GUIHandler.Instance.MessageProcessor.SendMessage(Enums.MessageType.Heartbeat, Enums.MessageAction.Ping, "PING");
                else
                    HandleServerDisconnected();
            }
            catch (Exception ex)
            {

                // Without Dispatcher, got error: System.InvalidOperationException: 'The calling thread must be STA, because many UI components require this.'

                Debug.WriteLine("Error in SendHeartbeat: " + ex.Message);
            }


            //if (_client != null && _client.Connected)
            //{
            //    try
            //    {

            //        GUIHandler.Instance.MessageProcessor.SendMessage(Enums.MessageType.Heartbeat, Enums.MessageAction.Ping, "PING");
            //    }
            //    catch
            //    {
            //        HandleServerDisconnected();
            //    }
            //}
            //else
            //{
            //    HandleServerDisconnected();
            //}
        }

        public async void HandleServerDisconnected()
        {
            Debug.WriteLine("Server disconnected.");
            Connected = false;
            if (System.Windows.Application.Current != null)
            {

                MessageBoxResult result = System.Windows.Application.Current.Dispatcher.Invoke(() => DXMessageBox.Show("Failed to connect to server. Do you want to try again?", "Error", MessageBoxButton.YesNo));
                if (result == MessageBoxResult.Yes)
                {

                    ConnectToServer();
                }
                if (result == MessageBoxResult.No)
                {

                    System.Windows.Application.Current.Dispatcher.Invoke(() => System.Windows.Application.Current.Shutdown());
                }
                //if (result.Equals(MessageBoxResult.Yes))
                //{
                //    //System.Windows.Application.Current.Dispatcher.Invoke(() => System.Windows.Application.Current.MainWindow.Close());
                //    ConnectToServer();
                //}
                if (_heartbeatTimer != null)
                {
                    _heartbeatTimer.Stop();
                }
            }
            //_heartbeatTimer.Stop();

        }


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
            }
        }

        //public void ReceiveMessage(Response response)
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
        //                        GUIHandler.Instance.CacheManager.AddCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Delete:

        //                        GUIHandler.Instance.CacheManager.DeleteCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Update:
        //                        GUIHandler.Instance.CacheManager.UpdateCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Load:
        //                        ObservableCollection<Category> categories = JsonSerializer.Deserialize<ObservableCollection<Category>>(response.Data.ToString());
        //                        GUIHandler.Instance.CacheManager.LoadCategories(categories);
        //                        break;
        //                }
        //                break;
        //            case Enums.MessageType.Product:
        //                switch (response.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        GUIHandler.Instance.CacheManager.AddProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Delete:
        //                        GUIHandler.Instance.CacheManager.DeleteProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Update:
        //                        GUIHandler.Instance.CacheManager.UpdateProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Load:
        //                        ObservableCollection<Product> products = JsonSerializer.Deserialize<ObservableCollection<Product>>(response.Data.ToString());
        //                        GUIHandler.Instance.CacheManager.LoadProducts(products);
        //                        break;
        //                }
        //                break;
        //            case Enums.MessageType.Order:
        //                switch (response.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        GUIHandler.Instance.CacheManager.AddOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Delete:
        //                        GUIHandler.Instance.CacheManager.DeleteOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Update:
        //                        GUIHandler.Instance.CacheManager.UpdateOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Load:
        //                        ObservableCollection<Order> orders = JsonSerializer.Deserialize<ObservableCollection<Order>>(response.Data.ToString());
        //                        GUIHandler.Instance.CacheManager.LoadOrders(orders);
        //                        break;
        //                }
        //                break;
        //            case Enums.MessageType.User:
        //                switch (response.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        GUIHandler.Instance.CacheManager.AddUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Delete:
        //                        GUIHandler.Instance.CacheManager.DeleteUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Update:
        //                        GUIHandler.Instance.CacheManager.UpdateUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
        //                        break;
        //                    case Enums.MessageAction.Load:
        //                        ObservableCollection<User> users = JsonSerializer.Deserialize<ObservableCollection<User>>(response.Data.ToString());
        //                        GUIHandler.Instance.CacheManager.LoadUsers(users);
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