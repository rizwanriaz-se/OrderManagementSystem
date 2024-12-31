using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;
using OrderManagementSystemServer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for EditCategoryView.xaml
    /// </summary>
    public partial class EditCategoryView : ThemedWindow
    {
        EditCategoryViewModel editCategoryViewModel = null;
        public EditCategoryView()
        {
            InitializeComponent();

            editCategoryViewModel = new EditCategoryViewModel();
            DataContext = editCategoryViewModel;
            editCategoryViewModel.CloseWindow = this.Close;
        }

        public void LoadCategory(Category SelectedCategory)
        {
            //if (SelectedCategory == null) throw new ArgumentNullException(nameof(SelectedCategory));

            editCategoryViewModel.Id = SelectedCategory.Id;
            editCategoryViewModel.CategoryNameText = SelectedCategory.Name;
            editCategoryViewModel.CategoryDescriptionText = SelectedCategory.Description;
            editCategoryViewModel.Picture = SelectedCategory.Picture;
        }
    }
}
