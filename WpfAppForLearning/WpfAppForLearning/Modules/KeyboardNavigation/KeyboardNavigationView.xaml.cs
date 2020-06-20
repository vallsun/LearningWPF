using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfAppForLearning.Modules.KeyboardNavigation
{
    /// <summary>
    /// KeyboardNavigationView.xaml の相互作用ロジック
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
    }
}
