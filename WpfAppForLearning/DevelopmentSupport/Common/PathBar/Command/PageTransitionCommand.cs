using DevelopmentSupport.Common.Hierarchical;
using DevelopmentSupport.Common.Namable;
using DevelopmentSupport.Common.Selectable;
using WPFAppFrameWork.Common;

namespace DevelopmentSupport.Common.PathBar.Command
{
	/// <summary>
	/// 要素選択で画面遷移するコマンド
	/// </summary>
	public class PageTransitionCommand<T> : DelegateCommand<PathBarItemViewModelBase<T>>
		where T : HierarchicalItemBase<T>, INamable, ISelectableItem
	{
		/// <summary>
		/// コマンド発行元のVM
		/// </summary>
		public PathBarItemViewModelBase<T> CommandSourceVM { get; set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public PageTransitionCommand(PathBarItemViewModelBase<T> vm)
			: base(null)
		{
			CommandSourceVM = vm;
		}

		public override bool CanExecute(PathBarItemViewModelBase<T> parameter)
		{
			return true;
		}

		public override void Execute(PathBarItemViewModelBase<T> parameter)
		{
			CommandSourceVM.Owner.SelectedItem = (T)CommandSourceVM.Model;
		}
	}
}
