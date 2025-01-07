using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditCategoryView.xaml
    /// </summary>
    public partial class EditCategoryView : ThemedWindow
    {
        private EditCategoryViewModel m_objEditCategoryViewModel = null;
        public EditCategoryView()
        {
            InitializeComponent();

            m_objEditCategoryViewModel = new EditCategoryViewModel();
            this.Owner = System.Windows.Application.Current.MainWindow;
            DataContext = m_objEditCategoryViewModel;
            m_objEditCategoryViewModel.CloseWindow = this.Close;
        }

        public void LoadCategory(Category SelectedCategory)
        {
            m_objEditCategoryViewModel.Id = SelectedCategory.Id;
            m_objEditCategoryViewModel.CategoryNameText = SelectedCategory.Name;
            m_objEditCategoryViewModel.CategoryDescriptionText = SelectedCategory.Description;
            m_objEditCategoryViewModel.Picture = SelectedCategory.Picture;
        }
    }
}
