using DevelopmentSupport.Common.Hierarchical;
using DevelopmentSupport.Common.Namable;
using DevelopmentSupport.Common.Selectable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common.PathBar
{
    /// <summary>
    /// パスバーのVMベース
    /// </summary>
    public class PathBarViewModelBase<T> : ViewModelBase
        where T : IHierarchicalItem, INamable
    {
        #region プロパティ

        /// <summary>
        /// パスリストのビューモデルのコレクション
        /// </summary>
        public ObservableCollection<PathBarItemViewModelBase<T>> PathListViewModel { get; set; }

        /// <summary>
        /// パスバーを所持するビューモデル
        /// </summary>
        public SelectableViewModelBase<T> Owner { get; set; }

        /// <summary>
        /// 現在選択中のアイテム
        /// </summary>
        public T SelectedItem
        {
            get => (T)Owner.SelectedItem;
            set => Owner.SelectedItem = value;
        }

        #endregion

        #region 構築・消滅

        //コンストラクタ
        public PathBarViewModelBase(SelectableViewModelBase<T> owner, T content)
        {
            Owner = owner;
            PathListViewModel = new ObservableCollection<PathBarItemViewModelBase<T>>();
            if (content != null)
            {
                CreatePathList(content);
            }
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// パスリストを生成
        /// </summary>
        /// <param name="content"></param>
        private void CreatePathList(IHierarchicalItem content)
        {
            Contract.Requires(PathListViewModel != null);

            //子要素から親要素に向かってリスト先頭に要素を追加
            PathListViewModel.Insert(0, new PathBarItemViewModelBase<T>(this, (T)content));
            if (content.Parent != null)
            {
                CreatePathList(content.Parent);
            };
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// 選択アイテムが変更された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Owner == null)
            {
                return;
            }
            PathListViewModel.Clear();
            //パスリストの再生成
            CreatePathList((IHierarchicalItem)Owner.SelectedItem);
        }

        #endregion
    }
}
