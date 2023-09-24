using DevelopmentSupport.Common.PathBar;
using WpfAppForLearning.Modules.ContentsTree.Model;
using WPFAppFrameWork;

namespace WpfAppForLearning.Modules.PathBarControl.ViewModel
{
	/// <summary>
	/// パスバーのビューモデル
	/// </summary>
	public class PathBarViewModelImpl : PathBarViewModelBase<Content>
	{
		#region 構築・消滅

		//コンストラクタ
		public PathBarViewModelImpl(SelectableViewModelBase<Content> owner, Content content)
			: base(owner, content)
		{
		}

		#endregion
	}
}
