//using DevExpress.Utils.Filtering.Internal;
//using DevExpress.Xpf.Core;
//using OrderManagementSystem.Cache;
//using OrderManagementSystemServer.Repository;
//using System;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Text.Json;


//namespace OrderManagementSystem.UIComponents.Classes
//{

//    //public class Request
//    //{
//    //    public MessageType MessageType { get; set; }
//    //    public MessageAction MessageAction { get; set; }
//    //    public object? Data { get; set; }
//    //}

//    //public class Response
//    //{
//    //    public MessageType MessageType { get; set; }
//    //    public MessageAction MessageAction { get; set; }
//    //    public object? Data { get; set; }
//    //    public string? Error { get; set; }
//    //}

//    public class MessageProcessor
//    {


//        //public static void SendMessage(MessageType messageType, MessageAction messageAction, object? message = null)
//        //{
//        //    Request request = new Request
//        //    {
//        //        MessageAction = messageAction,
//        //        MessageType = messageType,
//        //        Data = message
//        //    };


//        //    try
//        //    {
//        //        ClientManager.Instance.SendMessage(request);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Debug.WriteLine("Error in Client MessageProcessor SendMessage", ex.Message);
//        //    }
//        //}

//        //public static void ProcessCategoryMessage(Response response)
//        //{
//        //    try
//        //    {
//        //        Action action = () =>
//        //        {
//        //            switch (response.MessageAction)
//        //            {
//        //                case MessageAction.Add:
//        //                    CacheManager.Instance.AddCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
//        //                    break;
//        //                case MessageAction.Update:
//        //                    CacheManager.Instance.UpdateCategory(JsonSerializer.Deserialize<Category>(response.Data.ToString()));
//        //                    break;
//        //                case MessageAction.Delete:
//        //                    CacheManager.Instance.DeleteCategory(response.Data.ToString());
//        //                    break;
//        //                case MessageAction.Load:
//        //                    if (response.Data == null)
//        //                    {
//        //                        DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//        //                        return;
//        //                    }
//        //                    ObservableCollection<Category> categories = JsonSerializer.Deserialize<ObservableCollection<Category>>(response.Data.ToString());
//        //                    CacheManager.Instance.LoadCategories(categories);
//        //                    break;
//        //                default:
//        //                    break;
//        //            }
//        //        };

//        //        System.Windows.Application.Current.Dispatcher.BeginInvoke(action);

//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//        //    }
//        //}

//        //public static void ProcessOrderMessage(Response response)
//        //{
//        //    try
//        //    {
//        //        Action action = () =>
//        //        {

//        //            switch (response.MessageAction)
//        //            {
//        //                case MessageAction.Add:
//        //                    CacheManager.Instance.AddOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
//        //                    break;
//        //                case MessageAction.Delete:
//        //                    CacheManager.Instance.DeleteOrder(response.Data.ToString());
//        //                    break;
//        //                case MessageAction.Update:
//        //                    CacheManager.Instance.UpdateOrder(JsonSerializer.Deserialize<Order>(response.Data.ToString()));
//        //                    break;
//        //                case MessageAction.Load:
//        //                    if (response.Data == null)
//        //                    {
//        //                        DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//        //                        return;
//        //                    }
//        //                    ObservableCollection<Order> orders = JsonSerializer.Deserialize<ObservableCollection<Order>>(response.Data.ToString());
//        //                    CacheManager.Instance.LoadOrders(orders);
//        //                    break;
//        //            }
//        //        };

//        //        System.Windows.Application.Current.Dispatcher.BeginInvoke(action);

//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//        //    }
//        //}

//        //public static void ProcessProductMessage(Response response)
//        //{
//        //    try
//        //    {
//        //        Action action = () =>
//        //        {

//        //            switch (response.MessageAction)
//        //            {
//        //                case MessageAction.Add:
//        //                    CacheManager.Instance.AddProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
//        //                    break;
//        //                case MessageAction.Delete:
//        //                    CacheManager.Instance.DeleteProduct(response.Data.ToString());
//        //                    break;
//        //                case MessageAction.Update:
//        //                    CacheManager.Instance.UpdateProduct(JsonSerializer.Deserialize<Product>(response.Data.ToString()));
//        //                    break;
//        //                case MessageAction.Load:
//        //                    if (response.Data == null)
//        //                    {
//        //                        //throw new Exception("Error trying to load data from server. Please try again.");
//        //                        DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//        //                        return;
//        //                    }
//        //                        ObservableCollection<Product> products = JsonSerializer.Deserialize<ObservableCollection<Product>>(response.Data.ToString());
//        //                    CacheManager.Instance.LoadProducts(products);
//        //                    break;
//        //            }
//        //        };

//        //        System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//        //    }
//        //}

//        //public static void ProcessUserMessage(Response response)
//        //{
//        //    try
//        //    {
//        //        Action action = () =>
//        //        {
//        //            switch (response.MessageAction)
//        //            {
//        //                case MessageAction.Add:
//        //                    CacheManager.Instance.AddUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
//        //                    break;
//        //                case MessageAction.Delete:
//        //                    CacheManager.Instance.DeleteUser(response.Data.ToString());
//        //                    break;
//        //                case MessageAction.Update:
//        //                    CacheManager.Instance.UpdateUser(JsonSerializer.Deserialize<User>(response.Data.ToString()));
//        //                    break;
//        //                case MessageAction.Load:
//        //                    if (response.Data == null) {
//        //                        //throw new Exception("Error trying to load data from server. Please try again.");
//        //                        DXMessageBox.Show("Error trying to load data from server. Please try again.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//        //                        return;
//        //                    }
//        //                    ObservableCollection<User> users = JsonSerializer.Deserialize<ObservableCollection<User>>(response.Data.ToString());
//        //                    CacheManager.Instance.LoadUsers(users);
//        //                    break;
//        //            }
//        //        };

//        //        System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        DXMessageBox.Show(ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
//        //    }
//        //}
//    }
//}


