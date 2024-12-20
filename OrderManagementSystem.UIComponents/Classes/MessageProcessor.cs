using DevExpress.Office.Drawing;
using OrderManagementSystemServer.Repository;

//using OrderManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using static OrderManagementSystem.UIComponents.Classes;


namespace OrderManagementSystem.UIComponents.Classes
{

    public class Request
    {
        public Enums.MessageType MessageType { get; set; }
        public Enums.MessageAction MessageAction { get; set; }
        public object Data { get; set; }
    }

    public class Response
    {

        public Enums.MessageType MessageType { get; set; }
        public Enums.MessageAction MessageAction { get; set; }
        public object Data { get; set; }
    }

    public class MessageProcessor
    {
        //private ClientManager m_ClientManager;

        public MessageProcessor() { }
        //public MessageProcessor(ClientManager clientManager)
        //{
        //    m_ClientManager = clientManager;
        //}

        public void SendMessage(Enums.MessageType messageType, Enums.MessageAction messageAction, object message = null)
        {
            Request request = new Request
            {
                MessageAction = messageAction,
                MessageType = messageType,
                Data = message
            };
            try
            {
                GUIHandler.GetInstance().ClientManager.SendMessage(request);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Client MessageProcessor SendMessage", ex.Message);
            }
        }

        public static void ProcessCategoryMessage(Response response)
        {
            try
            {
                Action action = () =>
                {
                    switch (response.MessageAction)
                    {
                        case Enums.MessageAction.Add:
                            GUIHandler.GetInstance().CacheManager.AddCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Update:
                            GUIHandler.GetInstance().CacheManager.UpdateCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Delete:
                            GUIHandler.GetInstance().CacheManager.DeleteCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Load:
                            ObservableCollection<Category> categories = JsonSerializer.Deserialize<ObservableCollection<Category>>(response.Data.ToString());
                            GUIHandler.GetInstance().CacheManager.LoadCategories(categories);
                            break;
                        default:
                            break;
                    }
                };

                System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Client MessageProcessor ProcessCategoryMessage", ex.Message);
            }
        }

        public static void ProcessOrderMessage(Response response)
        {
            try
            {
                Action action = () =>
                {

                    switch (response.MessageAction)
                    {
                        case Enums.MessageAction.Add:
                            GUIHandler.GetInstance().CacheManager.AddOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Delete:
                            GUIHandler.GetInstance().CacheManager.DeleteOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Update:
                            GUIHandler.GetInstance().CacheManager.UpdateOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Load:
                            ObservableCollection<Order> orders = JsonSerializer.Deserialize<ObservableCollection<Order>>(response.Data.ToString());
                            GUIHandler.GetInstance().CacheManager.LoadOrders(orders);
                            break;
                    }
                };

                System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Client MessageProcessor ProcessOrderMessage", ex.Message);
            }
        }

        public static void ProcessProductMessage(Response response)
        {
            try
            {
                Action action = () =>
                {

                    switch (response.MessageAction)
                    {
                        case Enums.MessageAction.Add:
                            GUIHandler.GetInstance().CacheManager.AddProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Delete:
                            GUIHandler.GetInstance().CacheManager.DeleteProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Update:
                            GUIHandler.GetInstance().CacheManager.UpdateProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Load:
                            ObservableCollection<Product> products = JsonSerializer.Deserialize<ObservableCollection<Product>>(response.Data.ToString());
                            GUIHandler.GetInstance().CacheManager.LoadProducts(products);
                            break;
                    }
                };

                System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Client MessageProcessor ProcessProductMessage", ex.Message);
            }
        }

        public static void ProcessUserMessage(Response response)
        {
            try
            {
                Action action = () =>
                {
                    switch (response.MessageAction)
                    {
                        case Enums.MessageAction.Add:
                            GUIHandler.GetInstance().CacheManager.AddUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Delete:
                            GUIHandler.GetInstance().CacheManager.DeleteUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Update:
                            GUIHandler.GetInstance().CacheManager.UpdateUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Load:
                            ObservableCollection<User> users = JsonSerializer.Deserialize<ObservableCollection<User>>(response.Data.ToString());
                            GUIHandler.GetInstance().CacheManager.LoadUsers(users);
                            break;
                    }
                };

                System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Client MessageProcessor ProcessUserMessage", ex.Message);
            }
        }





        //public void ReceiveMessage(Enums.MessageType messageType, Enums.MessageAction messageAction, object message = null)
        //{
        //    Response response = new Response
        //    {
        //        MessageAction = messageAction,
        //        MessageType = messageType,
        //        Data = message
        //    };
        //    try
        //    {
        //        GUIHandler.GetInstance().ClientManager.ReceiveMessage(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Error in Client MessageProcessor SendMessage", ex.Message);
        //    }
        //}
    }
}


