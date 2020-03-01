using DevelopmentSupport.Common.Hierarchical;
using DevelopmentSupport.Common.Namable;
using DevelopmentSupport.Common.PathBar.Command;
using DevelopmentSupport.Common.Selectable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevelopmentSupport.Common.PathBar
{
    /// <summary>
    /// パスバー要素のVMベース
    /// </summary>
    public class PathBarItemViewModelBase<T> : ViewModelBase, ISelectableItem<T>, INamable
        where T : IHierarchicalItem, INamable
    {
        #region プロパティ

        /// <summary>
        /// オーナービューモデル
        /// </summary>
        public PathBarViewModelBase<T> OwnerVM { get; set; }

        /// <summary>
        /// 要素に対応するモデル
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

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
                return Model?.Children?.Any() ?? false;
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
        public PathBarItemViewModelBase(PathBarViewModelBase<T> vm, T model)
        {
            OwnerVM = vm;
            Model = model;
            Name = model.Name;

            SiblingListBuilder(model);
            RegisterCommands();
        }

        #endregion

        #region 初期化

        /// <summary>
        /// 兄弟リストの生成
        /// </summary>
        /// <param name="content"></param>
        private void SiblingListBuilder(T content)
        {
            SiblingList = new ObservableCollection<PathBarItemViewModelBase<T>>();

            if (content.Children == null)
            {
                return;
            };

            foreach (var contentItem in content.Children)
            {
                SiblingList.Add(new PathBarItemViewModelBase<T>(OwnerVM, (T)contentItem));
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
