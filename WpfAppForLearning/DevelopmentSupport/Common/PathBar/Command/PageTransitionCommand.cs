using DevelopmentSupport.Common;
using DevelopmentSupport.Common.Hierarchical;
using DevelopmentSupport.Common.Namable;
using DevelopmentSupport.Common.PathBar;

namespace DevelopmentSupport.Common.PathBar.Command
{
    /// <summary>
    /// 要素選択で画面遷移するコマンド
    /// </summary>
    public class PageTransitionCommand<T> : DelegateCommand<T>
        where T : IHierarchicalItem, INamable
    {
        /// <summary>
        /// コマンド発行元のVM
        /// </summary>
        public PathBarItemViewModelBase<T> CommandSourceVM { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PageTransitionCommand(PathBarItemViewModelBase<T> vm)
            :base(null)
        {
            CommandSourceVM = vm;
        }

        public override bool CanExecute(T parameter)
        {
            return true;
        }

        public override void Execute(T parameter)
        {
            CommandSourceVM.OwnerVM.SelectedItem = CommandSourceVM.Model;
        }
    }
}
