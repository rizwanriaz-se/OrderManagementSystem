using System.Collections.ObjectModel;
using DevExpress.Xpf.Core;
using OrderManagementSystemServer.Repository;


namespace OrderManagementSystem.Cache
{
    public class CacheManager
    {
        #region Declarations
        private ObservableCollection<Category> m_objCategories;
        private ObservableCollection<Order> m_lstOrders; //OrderList or Orders
        private ObservableCollection<Product> m_objProducts;
        private ObservableCollection<User> m_objUsers;
        private static CacheManager? m_objInstance;
        #endregion

        #region Initialization
        public static CacheManager Instance
        {
            get { 
                if (m_objInstance == null)
                    m_objInstance = new CacheManager();
                return m_objInstance;
            }
        }
        private CacheManager()
        {
            m_objUsers = new ObservableCollection<User>();
            m_objCategories = new ObservableCollection<Category>();
            m_objProducts = new ObservableCollection<Product>();
            m_lstOrders = new ObservableCollection<Order>();
        }
        #endregion
        
        #region CategoryMethods
        public void LoadCategories(ObservableCollection<Category> categories)
        {
            if (categories == null) return;
            m_objCategories = categories;
        }

        public ObservableCollection<Category> GetAllCategories()
        {
            return m_objCategories;
        }

        public void AddCategory(Category category)
        {
            m_objCategories.Add(category);
        }

        public void DeleteCategory(string category)
        {
            int categoryId = Int32.Parse(category);
            Category? categoryToDelete = m_objCategories.FirstOrDefault(c => c.Id == categoryId);
            if (categoryToDelete != null)
            {
                m_objCategories.Remove(categoryToDelete);
            }
        }

        public void UpdateCategory(Category category)
        {
            Category? categoryToUpdate = m_objCategories.FirstOrDefault(c => c.Id == category.Id);
            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.Description = category.Description;
                //categoryToUpdate.Picture = category.Picture;
            }
        }
        #endregion

        #region OrderMethods
        public void LoadOrders(ObservableCollection<Order> orders)
        {
            if (orders == null) return;
            m_lstOrders = orders;
        }

        public ObservableCollection<Order> GetAllOrders()
        {
            return m_lstOrders;
        }
       
        public void AddOrder(Order order)
        {
            m_lstOrders.Add(order);
        }
        public void UpdateOrder(Order updatedOrder)
        {
            // Retrieve the existing order with the same ID
            Order? existingOrder = m_lstOrders.FirstOrDefault(o => o.Id == updatedOrder.Id);

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
        public void DeleteOrder(string order)
        {
            int orderId = Int32.Parse(order);
            Order? orderToDelete = m_lstOrders.FirstOrDefault(o => o.Id == orderId);
            if (orderToDelete != null)
            {
                m_lstOrders.Remove(orderToDelete);
            }
        }
        #endregion

        #region ProductMethods
        public void LoadProducts(ObservableCollection<Product> products)
        {
            if (products == null) return;
            m_objProducts = products;
        }

        public ObservableCollection<Product> GetAllProducts()
        {
            return m_objProducts;
        }
       
        public void AddProduct(Product product)
        {
            m_objProducts.Add(product);
        }
        public void UpdateProduct(Product product)
        {
            Product? existingProduct = m_objProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.UnitsInStock = product.UnitsInStock;
            }
        }
        public void DeleteProduct(string product)
        {
            int productId = Int32.Parse(product);
            Product? productToDelete = m_objProducts.FirstOrDefault(p => p.Id == productId);
            if (productToDelete != null)
            {
                m_objProducts.Remove(productToDelete);
            }
        }
        #endregion

        #region UserMethods
        public void LoadUsers(ObservableCollection<User> users)
        {
            if (users == null) return;
            m_objUsers = users;
        }

        public ObservableCollection<User> GetAllUsers()
        {
            return m_objUsers;
            
        }

        public void AddUser(User user)
        {
            m_objUsers.Add(user);
            DXMessageBox.Show("Registration successful!! Your account is pending approval.");
        }
        
        public void UpdateUser(User user)
        {
            User? existingUser = m_objUsers.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Password = user.Password;
                existingUser.IsAdmin = user.IsAdmin;
                existingUser.ApprovalStatus = user.ApprovalStatus;
                existingUser.IsArchived = user.IsArchived;
            }
        }

        public void DeleteUser(string user)
        {
            int userId = Int32.Parse(user);
            User? userToDelete = m_objUsers.FirstOrDefault(u => u.Id == userId);

            userToDelete.IsArchived = true;

        }
        #endregion

    }
}
