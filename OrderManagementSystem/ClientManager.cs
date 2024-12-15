﻿using DevExpress.Pdf.Native.BouncyCastle.Asn1.Ocsp;
using DevExpress.XtraRichEdit.Fields.Expression;
//using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using OrderManagementSystem.Cache.Models;
using System.Text.Json;
using System.Diagnostics;
//using System.ServiceModel.Channels;

namespace OrderManagementSystem
{

    //public class Request
    //{
    //    public string Type { get; set; }
    //    public string DATA { get; set; }
    //}

    //public class Response
    //{
    //    public string Type
    //    {
    //        get; set;
    //    }
    //    public string DATA
    //    {
    //        get; set;
    //    }
    //}

        //public enum Type
        //{
        //    USER,


        //}

        public class ClientManager
    {
        private Timer _heartbeatTimer;
        private TcpClient _client;

        public TcpClient Client
        {
            get { return _client; }
        }

        private NetworkStream _stream;

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
                    onConnected?.Invoke();
                }

                _stream = _client.GetStream();

                //InitializeHeartbeat();
                ListenAsync();

                //_heartbeatTimer.Start();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }

        private async Task ListenAsync()
        {
            Debug.WriteLine("START LISTEN");
            try
            {
                while (Connected)
                {
                    byte[] buffer = new byte[2048];
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.WriteLine($"Received on client: {response}");
                    //Request request = JsonSerializer.Deserialize<Response>(response);
                    ProcessResponse(response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in listener: " + ex.Message);
            }
        }

        private void ProcessResponse(string response)
        {
            try
            {
                Classes.Response resp = JsonSerializer.Deserialize<Classes.Response>(response);
                GUIHandler.GetInstance().MessageProcessor.ReceiveMessage(resp.MessageType, resp.MessageAction, resp.Data);
                //switch (resp.MessageType)
                //{
                //    case Enums.MessageType.Category:
                //        //User user = JsonSerializer.Deserialize<Category>(response);
                //        Debug.WriteLine(resp);
                //        //GUIHandler.GetInstance().CacheManager.AddCategory()

                //        Console.WriteLine($"Data is: {resp}");
                //        break;

                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing response: {ex.Message}");
            }
        }

        //public void InitializeHeartbeat()
        //{
        //    _heartbeatTimer = new Timer(5000); // 5 seconds interval
        //    _heartbeatTimer.Elapsed += async (sender, e) => await SendHeartbeat();
        //    _heartbeatTimer.AutoReset = true;
        //}


        //private async Task SendHeartbeat()
        //{
        //    if (_client != null && _client.Connected)
        //    {
        //        try
        //        {
        //            string pingRequest = JsonSerializer.Serialize(new Classes.Request
        //            {
        //                MessageAction = Enums.MessageAction.HeartBeat,
        //                MessageType = Enums.MessageType.HeartBeat,
        //                Data = "PING"
        //            });


        //            byte[] message = Encoding.ASCII.GetBytes(pingRequest);
        //            await _stream.WriteAsync(message, 0, message.Length);

        //            byte[] responseBuffer = new byte[2048];
        //            int bytesRead = await _stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

        //            string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
        //            if (response != "PONG") HandleServerDisconnected();
        //        }
        //        catch
        //        {
        //            HandleServerDisconnected();
        //        }
        //    }
        //}

        //private void HandleServerDisconnected()
        //{
        //    Console.WriteLine("Server disconnected.");
        //    Connected = false;
        //    _heartbeatTimer.Stop();
        //}

        //public User SendLoginRequest(string email, string password)
        //{

        //    //if (_client == null || !_client.Connected) return;
        //    var loginMessage = new Request
        //    {
        //        Type = "LOGIN",
        //        DATA = JsonSerializer.Serialize(new User { Email = email, Password = password })
        //    };

        //    SendMessage(loginMessage);
        //    return new User();
        //    //Response request = ReceiveMessage();
        //    //return request;


        //    //string request = $"LOGIN:{email}:{password}";
        //    //byte[] requestBytes = Encoding.ASCII.GetBytes(request);
        //    //await _stream.WriteAsync(requestBytes, 0, requestBytes.Length);

        //    //byte[] responseBuffer = new byte[2048];
        //    //int bytesRead = await _stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
        //    //string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
        //    //Debug.WriteLine(response);
        //    //response = response.Replace("\0", string.Empty);

        //    //return JsonSerializer.Deserialize<User>(response);
        //    //Console.WriteLine($"Login response: {response}");
        //}

        public async Task SendMessage(Classes.Request request)
        {
            string json = JsonSerializer.Serialize(request);
            byte[] data = Encoding.UTF8.GetBytes(json);
            await _stream.WriteAsync(data, 0, data.Length);
        }
        
        public void ReceiveMessage(Classes.Response response)
        {
            Debug.WriteLine($"Data Received on client: {response.Data}");

            // Deserialize response.Data to Category type
            var category = JsonSerializer.Deserialize<Category>(response.Data.ToString());
            if (category != null)
            {
                GUIHandler.GetInstance().CacheManager.AddCategory(category);
            }
            else
            {
                Debug.WriteLine("Failed to deserialize response.Data to Category.");
            }
        }

        //internal void SendRequest(UIComponents.ViewModels.Request request)
        //{
        //    throw new NotImplementedException();
        //}
    }
}




//private async Task SendHeartbeat()
//{
//    if (_client != null && _client.Connected)
//    {
//        try
//        {
//            //_stream =
//            byte[] heartbeatMessage = Encoding.ASCII.GetBytes("PING");
//            await _stream.WriteAsync(heartbeatMessage, 0, heartbeatMessage.Length);
//            Console.WriteLine("Sent by client: PING");

//            byte[] responseBuffer = new byte[2048];
//            int bytesRead = await _stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
//            if (bytesRead > 0)
//            {
//                string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
//                Console.WriteLine("Received on client: " + response);

//                if (response != "PONG")
//                {
//                    Console.WriteLine($"Heartbeat failed");
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Heartbeat error: {ex.Message}");
//            _heartbeatTimer.Stop();

//        }
//    }
//}

//public void DisconnectFromServer() {

//    // Disconnect from server
//}