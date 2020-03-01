using DevelopmentSupport.Common.PathBar;
using DevelopmentSupport.Common.PathBar.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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
