using System.Collections.ObjectModel;
using System.Linq;
using OrderManagementSystem.Repositories.Repositories;

namespace OrderManagementSystem.Cache
{
    public class CacheManager
    {
        public ObservableCollection<Category> _AllCategories { get; private set; }
        public ObservableCollection<Order> _AllOrders { get; private set; }
        public ObservableCollection<Product> _AllProducts { get; private set; }
        public ObservableCollection<User> _AllUsers { get; private set; }

        private User m_CurrentUser;

        private static CacheManager m_Instance;
        public static CacheManager Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = new CacheManager();
            }
            return m_Instance;
        }

        public User CurrentUser
        {
            get { return m_CurrentUser; }
            set { m_CurrentUser = value; }
        }

        public void LoadCategories(ObservableCollection<Category> categories)
        {
            if (categories == null) return;
            _AllCategories = categories;
        }

        public void LoadOrders(ObservableCollection<Order> orders)
        {
            if (orders == null) return;
            _AllOrders = orders;
        }

        public void LoadProducts(ObservableCollection<Product> products)
        {
            if (products == null) return;
            _AllProducts = products;
        }

        public void LoadUsers(ObservableCollection<User> users)
        {
            if (users == null) return;
            _AllUsers = users;
        }

        private CacheManager()
        {

            _AllUsers = new ObservableCollection<User>();

            _AllCategories = new ObservableCollection<Category>();

            _AllProducts = new ObservableCollection<Product>();

            _AllOrders = new ObservableCollection<Order>();
        }

        public Category GetCategoryById(int id)
        {
            return _AllCategories.FirstOrDefault(c => c.Id == id);
        }
        public ObservableCollection<Category> GetAllCategories()
        {
            return _AllCategories;
        }
        public void AddCategory(Category category)
        {
            _AllCategories.Add(category);
        }
        public void DeleteCategory(Category category)
        {
            _AllCategories.Remove(category);
        }
        public void UpdateCategory(Category category)
        {
            var categoryToUpdate = _AllCategories.FirstOrDefault(c => c.Id == category.Id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.Picture = category.Picture;
        }
        public ObservableCollection<Order> GetAllOrders()
        {
            return _AllOrders;
        }
        public Order GetOrderById(int id)
        {
            return _AllOrders.FirstOrDefault(o => o.Id == id);
        }
        public void AddOrder(Order order)
        {
            _AllOrders.Add(order);
        }
        public void UpdateOrder(Order updatedOrder)
        {
            // Retrieve the existing order with the same ID
            var existingOrder = _AllOrders.FirstOrDefault(o => o.Id == updatedOrder.Id);

            if (existingOrder != null)
            {
                // Update the existing order's properties
                existingOrder.User = updatedOrder.User;
                existingOrder.OrderDate = updatedOrder.OrderDate;
                existingOrder.Status = updatedOrder.Status;
                existingOrder.ShippedDate = updatedOrder.ShippedDate;
                existingOrder.ShippingAddress = updatedOrder.ShippingAddress;
                existingOrder.OrderDetails = updatedOrder.OrderDetails;
            }
            else
            {
                //MessageBox.Show("Order not found.");
            }
        }
        public void DeleteOrder(Order order)
        {
            _AllOrders.Remove(order);
        }
        public ObservableCollection<Product> GetAllProducts()
        {
            return _AllProducts;
        }
        public Product GetProductByID(int id)
        {

            return _AllProducts.FirstOrDefault(p => p.Id == id);
        }
        public Product GetProductByName(OrderDetail orderDetail)
        {


            return _AllProducts.FirstOrDefault(p => p.Name == orderDetail.Product.Name);
        }
        public void AddProduct(Product product)
        {
            _AllProducts.Add(product);
        }
        public void UpdateProduct(Product product)
        {
            var existingProduct = _AllProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.Picture = product.Picture;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.UnitsInStock = product.UnitsInStock;

                //MessageBox.Show($"Product Updated: {existingProduct.Id}, {existingProduct.Name}, {existingProduct.Description}, {existingProduct.Category.Name}, {existingProduct.UnitPrice}, {existingProduct.UnitsInStock}");
            }
        }
        public void DeleteProduct(Product product)
        {
            _AllProducts.Remove(product);
            
        }
        public ObservableCollection<User> GetAllUsers()
        {
            return _AllUsers;
        }

        public void AddUser(User user)
        {
            _AllUsers.Add(user);
            //SaveData(true);
        }
        public User GetUserByID(int id)
        {
            return _AllUsers.FirstOrDefault(u => u.Id == id);
        }
        public void UpdateUser(User user)
        {
            var existingUser = _AllUsers.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Password = user.Password;
                existingUser.IsAdmin = user.IsAdmin;

                //MessageBox.Show($"Product Updated: {existingProduct.Id}, {existingProduct.Name}, {existingProduct.Description}, {existingProduct.Category.Name}, {existingProduct.UnitPrice}, {existingProduct.UnitsInStock}");
            }
        }

        public void DeleteUser(User user)
        {
            _AllUsers.Remove(user);
        }
    }
}
