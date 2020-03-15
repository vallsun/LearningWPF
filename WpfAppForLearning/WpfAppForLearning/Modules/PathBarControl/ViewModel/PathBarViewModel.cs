using DevelopmentSupport.Common.PathBar;
using DevelopmentSupport.Common.Selectable;
using WpfAppForLearning.Modules.ContentsTree.Model;

namespace WpfAppForLearning.Modules.PathBarControl.ViewModel
{
    /// <summary>
    /// パスバーのビューモデル
    /// </summary>
    public class PathBarViewModel : PathBarViewModelBase<Content>
    {
        #region 構築・消滅

        //コンストラクタ
        public PathBarViewModel(SelectableViewModelBase<Content> owner, Content content)
            : base(owner, content)
        {
        }

        #endregion
    }
}
