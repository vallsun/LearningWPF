using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAppForLearning.Modules.PathBarControl.ViewModel;

namespace WpfAppForLearning.Modules.PathBarControl.Command
{
    /// <summary>
    /// 要素選択で画面遷移するコマンド
    /// </summary>
    class PageTransitionCommand : ICommand
    {
        /// <summary>
        /// コマンド発行元のVM
        /// </summary>
        PathBarItemViewModel CommandSourceVM{ get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PageTransitionCommand(PathBarItemViewModel vm)
        {
            CommandSourceVM = vm;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CommandSourceVM.OwnerVM.SelectedItem = CommandSourceVM.Model;
        }
    }
}
