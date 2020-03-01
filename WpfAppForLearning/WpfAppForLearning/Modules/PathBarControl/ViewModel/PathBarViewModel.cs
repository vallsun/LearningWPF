using DevelopmentSupport.Common.PathBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppForLearning.Modules.ContentsTree.Model;
using WpfAppForLearning.ViewModel;

namespace WpfAppForLearning.Modules.PathBarControl.ViewModel
{
    /// <summary>
    /// パスバーのビューモデル
    /// </summary>
    public class PathBarViewModel : PathBarViewModelBase<Content>
    {
        #region 構築・消滅

        //コンストラクタ
        public PathBarViewModel(MainViewModel owner, Content content)
            : base(owner, content)
        {
        }

        #endregion
    }
}
