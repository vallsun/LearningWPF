using DevelopmentSupport.Common.Hierarchical;
using DevelopmentSupport.Common.Namable;
using DevelopmentSupport.Common.PathBar.Command;
using DevelopmentSupport.Common.Selectable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DevelopmentSupport.Common.PathBar
{
    /// <summary>
    /// パスバー要素のVMベース
    /// </summary>
    public class PathBarItemViewModelBase<T> : ViewModelBase, ISelectableItem
        where T : HierarchicalItemBase<T>, INamable, ISelectableItem
    {
        #region プロパティ

        /// <summary>
        /// オーナービューモデル
        /// </summary>
        public PathBarViewModelBase<T> Owner { get; set; }

        #region IHierarchicalItemメンバ

        /// <summary>
        /// 親
        /// </summary>
        public PathBarItemViewModelBase<T> Parent { get; set; }

        /// <summary>
        /// 子要素のコレクション
        /// </summary>
        public ObservableCollection<PathBarItemViewModelBase<T>> Children { get; set; }

        #endregion

        #region ISelectableItemメンバ

        /// <summary>
        /// 選択されているか
        /// </summary>
        public bool IsSelected { get; set; }

        #endregion

        #region INamableメンバ

        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get { return ((T)Model).Name; }
        }

        #endregion

        /// <summary>
        /// 兄弟リスト
        /// </summary>
        public ObservableCollection<PathBarItemViewModelBase<T>> SiblingList { get; set; }

        /// <summary>
        /// 子要素を持つか
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return ((T)Model).Children?.Any() ?? false;
            }
        }

        /// <summary>
        /// ページ遷移コマンド
        /// </summary>
        public ICommand PageTransisitonCommand { get; set; }

        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content"></param>
        public PathBarItemViewModelBase(PathBarViewModelBase<T> vm, HierarchicalItemBase<T> model)
            : base(model)
        {
            Owner = vm;
            SiblingListBuilder(model);
            RegisterCommands();
        }

        #endregion

        #region 初期化

        /// <summary>
        /// 兄弟リストの生成
        /// </summary>
        /// <param name="content"></param>
        private void SiblingListBuilder(HierarchicalItemBase<T> content)
        {
            SiblingList = new ObservableCollection<PathBarItemViewModelBase<T>>();

            if (content.Children == null)
            {
                return;
            };

            foreach (var contentItem in content.Children)
            {
                
                SiblingList.Add(new PathBarItemViewModelBase<T>(Owner, contentItem));
            }
        }

        /// <summary>
        /// コマンドの登録
        /// </summary>
        internal override void RegisterCommands()
        {
            base.RegisterCommands();


            //コマンドのインスタンス化
            PageTransisitonCommand = new PageTransitionCommand<T>(this);

        }

        #endregion
    }
}
