using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppForLearning.Modules.KeyboardNavigation
{
    /// <summary>
    /// TabControl.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyboardNavigationView : UserControl
    {
        public KeyboardNavigationView()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = comboBox.SelectedItem as ComboBoxItem;
            var name = selectedItem.Content.ToString();
            var vm = DataContext as KeyboardNavigationViewModel;
            if(comboBox == ComboBox1)
            {
                vm.NavigationMode1 = name;
            }
            else
            {
                vm.NavigationMode2 = name;
            }
            
        }

        private void hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;

        }
    }
}
