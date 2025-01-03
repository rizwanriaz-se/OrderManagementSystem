using System.Collections.ObjectModel;
using System.Linq;
//using OrderManagementSystem.Repository;
using OrderManagementSystemServer.Repository;

namespace OrderManagementSystem.Cache
{
    public class CacheManager
    {
        // change naming convention like _AllCategories to objCategories
        // make them member variable initially as null, and private setter removal


        private ObservableCollection<Category> m_objCategories;
        private ObservableCollection<Order> m_objOrders;
        private ObservableCollection<Product> m_objProducts;
        private ObservableCollection<User> m_objUsers;

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
            m_objCategories = categories;
        }

        public void LoadOrders(ObservableCollection<Order> orders)
        {
            if (orders == null) return;
            m_objOrders = orders;
        }

        public void LoadProducts(ObservableCollection<Product> products)
        {
            if (products == null) return;
            m_objProducts = products;
        }

        public void LoadUsers(ObservableCollection<User> users)
        {
            if (users == null) return;
            m_objUsers = users;
        }

        private CacheManager()
        {
            m_objUsers = new ObservableCollection<User>();
            m_objCategories = new ObservableCollection<Category>();
            m_objProducts = new ObservableCollection<Product>();
            m_objOrders = new ObservableCollection<Order>();
        }

        public Category GetCategoryById(int id)
        {
            return m_objCategories.FirstOrDefault(c => c.Id == id);
        }
        public ObservableCollection<Category> GetAllCategories()
        {
            return m_objCategories;
        }
        public void AddCategory(Category category)
        {
            m_objCategories.Add(category);
        }
        public void DeleteCategory(Category category)
        {
            Category categoryToDelete = m_objCategories.FirstOrDefault(c => c.Id == category.Id);
            if (categoryToDelete != null)
            {
                m_objCategories.Remove(categoryToDelete);
            }
            m_objCategories.Remove(category);
        }
        public void UpdateCategory(Category category)
        {
            var categoryToUpdate = m_objCategories.FirstOrDefault(c => c.Id == category.Id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.Picture = category.Picture;
        }
        public ObservableCollection<Order> GetAllOrders()
        {
            return m_objOrders;
        }
        public Order GetOrderById(int id)
        {
            return m_objOrders.FirstOrDefault(o => o.Id == id);
        }
        public void AddOrder(Order order)
        {
            m_objOrders.Add(order);
        }
        public void UpdateOrder(Order updatedOrder)
        {
            // Retrieve the existing order with the same ID
            var existingOrder = m_objOrders.FirstOrDefault(o => o.Id == updatedOrder.Id);

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
        }
        public void DeleteOrder(Order order)
        {
            Order orderToDelete = m_objOrders.FirstOrDefault(o => o.Id == order.Id);
            if (orderToDelete != null)
            {
                m_objOrders.Remove(orderToDelete);
            }

            // try this too later on
            //if (m_objOrders.Contains(order))
            //{
            //    m_objOrders.Remove(order);
            //}
            //m_objOrders.Remove(order);
        }
        public ObservableCollection<Product> GetAllProducts()
        {
            return m_objProducts;
        }
        public Product GetProductByID(int id)
        {

            return m_objProducts.FirstOrDefault(p => p.Id == id);
        }
        public Product GetProductByName(OrderDetail orderDetail)
        {


            return m_objProducts.FirstOrDefault(p => p.Name == orderDetail.Product.Name);
        }
        public void AddProduct(Product product)
        {
            m_objProducts.Add(product);
        }
        public void UpdateProduct(Product product)
        {
            var existingProduct = m_objProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.Picture = product.Picture;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.UnitsInStock = product.UnitsInStock;
            }
        }
        public void DeleteProduct(Product product)
        {
            Product productToDelete = m_objProducts.FirstOrDefault(p => p.Id == product.Id);
            if (productToDelete != null)
            {
                m_objProducts.Remove(productToDelete);
            }
        }

        public ObservableCollection<User> GetAllUsers()
        {
            return m_objUsers;
        }

        public void AddUser(User user)
        {
            m_objUsers.Add(user);
        }
        public User GetUserByID(int id)
        {
            return m_objUsers.FirstOrDefault(u => u.Id == id);
        }
        public void UpdateUser(User user)
        {
            var existingUser = m_objUsers.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Password = user.Password;
                existingUser.IsAdmin = user.IsAdmin;
            }
        }

        public void DeleteUser(User user)
        {
            User userToDelete = m_objUsers.FirstOrDefault(u => u.Id == user.Id);
            if (userToDelete != null)
            {
                m_objUsers.Remove(userToDelete);
            }            
        }
    }
}
