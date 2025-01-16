using System.Collections.ObjectModel;
using DevExpress.Xpf.Core;
using OrderManagementSystemServer.Repository;


namespace OrderManagementSystem.Cache
{
    public class CacheManager
    {
        #region Declarations
        private ObservableCollection<Category>? m_lstCategories;
        private ObservableCollection<Order>? m_lstOrders; //OrderList or Orders
        private ObservableCollection<Product>? m_lstProducts;
        private ObservableCollection<User>? m_lstUsers;
        private static CacheManager? m_objInstance;
        #endregion

        public ObservableCollection<Order>? Orders
        {
            get { if (m_lstOrders != null) return m_lstOrders; return null; }
            set { m_lstOrders = value; }
        }
        public ObservableCollection<Category>? Categories
        {
            get { if (m_lstCategories != null) return m_lstCategories; return null; }
            set { m_lstCategories = value; }
        }
        public ObservableCollection<Product>? Products
        {
            get { if (m_lstProducts != null) return m_lstProducts; return null; }
            set { m_lstProducts = value; }
        }
        public ObservableCollection<User>? Users
        {
            get { if (m_lstUsers != null) return m_lstUsers; return null; }
            set { m_lstUsers = value; }
        }

        #region Initialization
        public static CacheManager Instance
        {
            get
            {
                if (m_objInstance == null)
                    m_objInstance = new CacheManager();
                return m_objInstance;
            }
        }
        private CacheManager()
        {
            Users = new ObservableCollection<User>();
            Categories = new ObservableCollection<Category>();
            Products = new ObservableCollection<Product>();
            Orders = new ObservableCollection<Order>();
        }
        #endregion

        #region CategoryMethods
        public void AddCategory(Category category)
        {
            if (category != null)
                Categories.Add(category);
        }

        public void DeleteCategory(string category)
        {
            if (category != null)
            {
                int categoryId = Int32.Parse(category);
                Category? categoryToDelete = Categories.FirstOrDefault(c => c.Id == categoryId);
                if (categoryToDelete != null)
                {
                    Categories.Remove(categoryToDelete);
                }
            }
        }

        public void UpdateCategory(Category category)
        {
            if (category != null)
            {
                Category? categoryToUpdate = Categories.FirstOrDefault(c => c.Id == category.Id);
                if (categoryToUpdate != null)
                {
                    categoryToUpdate.Name = category.Name;
                    categoryToUpdate.Description = category.Description;
                }
            }
        }
        #endregion

        #region OrderMethods



        public void AddOrder(Order order)
        {
            if (order != null) { Orders.Add(order); }

        }
        public void UpdateOrder(Order updatedOrder)
        {
            if (updatedOrder != null)
            {
                // Retrieve the existing order with the same ID
                Order? existingOrder = Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);

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
        }
        public void DeleteOrder(string order)
        {
            if (order != null)
            {
                int orderId = Int32.Parse(order);
                Order? orderToDelete = Orders.FirstOrDefault(o => o.Id == orderId);

                if (orderToDelete != null)
                {
                    Orders.Remove(orderToDelete);
                }
            }
        }
        #endregion

        #region ProductMethods
        public void AddProduct(Product product)
        {
            if (product != null)
                Products.Add(product);
            
        }
        public void UpdateProduct(Product product)
        {
            if (product != null)
            {
                Product? existingProduct = Products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Category = product.Category;
                    existingProduct.UnitPrice = product.UnitPrice;
                    existingProduct.UnitsInStock = product.UnitsInStock;
                }
            }
        }
        public void DeleteProduct(string product)
        {
            if (product != null)
            {
                int productId = Int32.Parse(product);
                Product? productToDelete = Products.FirstOrDefault(p => p.Id == productId);
                if (productToDelete != null)
                {
                    Products.Remove(productToDelete);
                }
            }
        }
        #endregion

        #region UserMethods
        public void AddUser(User user)
        {

            if (user != null)
            {
                Users.Add(user);
                DXMessageBox.Show("Registration successful!! Your account is pending approval.");
            }
        }

        public void UpdateUser(User user)
        {
            if (user != null)
            {
                User? existingUser = Users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.Phone = user.Phone;
                    existingUser.Password = user.Password;
                    existingUser.IsAdmin = user.IsAdmin;
                    existingUser.UserApprovalStatus = user.UserApprovalStatus;
                    existingUser.IsArchived = user.IsArchived;
                }
            }
        }

        public void DeleteUser(string user)
        {
            if (user != null)
            {
                int userId = Int32.Parse(user);
                User? userToDelete = Users.FirstOrDefault(u => u.Id == userId);

                userToDelete.IsArchived = true;
            }
        }
        #endregion

    }
}
