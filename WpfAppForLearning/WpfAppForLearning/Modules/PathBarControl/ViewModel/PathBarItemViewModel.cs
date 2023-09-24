using DevelopmentSupport.Common.PathBar;
using WpfAppForLearning.Modules.ContentsTree.Model;

namespace WpfAppForLearning.Modules.PathBarControl.ViewModel
{
	public class PathBarItemViewModel : PathBarItemViewModelBase<Content>
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="content"></param>
		public PathBarItemViewModel(PathBarViewModel vm, Content content)
			: base(vm, content)
		{

		}
	}
}
