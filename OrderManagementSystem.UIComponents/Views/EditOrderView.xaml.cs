using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;
using System.Collections.ObjectModel;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditOrderView.xaml
    /// </summary>
    public partial class EditOrderView : ThemedWindow
    {
        private EditOrderViewModel m_objEditOrderViewModel;
        public EditOrderView()
        {
            InitializeComponent();
            m_objEditOrderViewModel = new EditOrderViewModel();
            this.Owner = System.Windows.Application.Current.MainWindow;
            DataContext = m_objEditOrderViewModel;
            m_objEditOrderViewModel.CloseWindow = this.Close;
        
        }
       
        public void LoadOrder(Order SelectedOrder)
        {
            if (SelectedOrder == null) throw new ArgumentNullException(nameof(SelectedOrder));

            m_objEditOrderViewModel.Id = SelectedOrder.Id;
            m_objEditOrderViewModel.User = SelectedOrder.User ?? throw new ArgumentNullException(nameof(SelectedOrder.User));
            m_objEditOrderViewModel.OrderDate = SelectedOrder.OrderDate;
            m_objEditOrderViewModel.SelectedShippingDate = SelectedOrder.ShippedDate;
            m_objEditOrderViewModel.SelectedStatus = SelectedOrder.Status;
            m_objEditOrderViewModel.SelectedShippingAddress = SelectedOrder.ShippingAddress ?? throw new ArgumentNullException(nameof(SelectedOrder.ShippingAddress));
            m_objEditOrderViewModel.OrderDetails = new ObservableCollection<OrderDetail>(SelectedOrder.OrderDetails.Select(od =>
                new OrderDetail
                {
                    Product =  m_objEditOrderViewModel.AllProducts.FirstOrDefault(p => p.Id == od.Product.Id),
                    Quantity = od.Quantity
                }));

            

            m_objEditOrderViewModel.OrderDetails.CollectionChanged += (s, e) => m_objEditOrderViewModel.SaveOrderCommand.RaiseCanExecuteEventChanged();
        }
    }
}
