using WpfAppForLearning.Modules.ContentsTree.Model;
using WPFAppFrameWork;

namespace WpfAppForLearning.Modules.PathBarControl.ViewModel
{
	public class PathBarItemViewModelImpl : PathBarItemViewModelBase<Content>
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="content"></param>
		public PathBarItemViewModelImpl(PathBarViewModelImpl vm, Content content)
			: base(vm, content)
		{

		}
	}
}
