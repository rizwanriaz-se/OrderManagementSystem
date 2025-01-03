using DevExpress.Office.Drawing;
//using OrderManagementSystem.Cache;
using OrderManagementSystemServer.Repository;

//using OrderManagementSystemServer.Repository;

//using OrderManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.Cache;
using DevExpress.Xpf.Core;


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
        public object? Data { get; set; }
        public string? Error { get; set; }
    }

    public class MessageProcessor
    {

        public MessageProcessor() { }

        public static void SendMessage(Enums.MessageType messageType, Enums.MessageAction messageAction, object message = null)
        {
            Request request = new Request
            {
                MessageAction = messageAction,
                MessageType = messageType,
                Data = message
            };
            try
            {
                ClientManager.Instance().SendMessage(request);
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
                            CacheManager.Instance().AddCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Update:
                            CacheManager.Instance().UpdateCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Delete:
                            CacheManager.Instance().DeleteCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Load:
                            ObservableCollection<Category> categories = JsonSerializer.Deserialize<ObservableCollection<Category>>(response.Data.ToString());
                            CacheManager.Instance().LoadCategories(categories);
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
                            CacheManager.Instance().AddOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Delete:
                            CacheManager.Instance().DeleteOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Update:
                            CacheManager.Instance().UpdateOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Load:
                            ObservableCollection<Order> orders = JsonSerializer.Deserialize<ObservableCollection<Order>>(response.Data.ToString());
                            CacheManager.Instance().LoadOrders(orders);
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
                            CacheManager.Instance().AddProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Delete:
                            CacheManager.Instance().DeleteProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Update:
                            CacheManager.Instance().UpdateProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Load:
                            ObservableCollection<Product> products = JsonSerializer.Deserialize<ObservableCollection<Product>>(response.Data.ToString());
                            CacheManager.Instance().LoadProducts(products);
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
                            CacheManager.Instance().AddUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Delete:
                            CacheManager.Instance().DeleteUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Update:
                            CacheManager.Instance().UpdateUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
                            break;
                        case Enums.MessageAction.Load:
                            ObservableCollection<User> users = JsonSerializer.Deserialize<ObservableCollection<User>>(response.Data.ToString());
                            CacheManager.Instance().LoadUsers(users);
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
    }
}


