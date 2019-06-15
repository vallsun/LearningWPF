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
    public class PathBarViewModel
    {
        #region プロパティ

        /// <summary>
        /// パスリストのモデルのコレクション
        /// </summary>
        public ObservableCollection<Content> PathList { get; set; }

        /// <summary>
        /// パスリストのビューモデルのコレクション
        /// </summary>
        public ObservableCollection<PathBarItemViewModel> PathListViewModel { get; set; }

        /// <summary>
        /// パスバーを所持するビューモデル
        /// </summary>
        public MainViewModel Owner { get; set; }

        /// <summary>
        /// 現在選択中のアイテム
        /// </summary>
        public Content SelectedItem
        {
            get
            {
                return Owner.SelectedItem;
            }

            set
            {
                Owner.SelectedItem = value;
            }
        }

        #endregion

        #region 構築・消滅

        //コンストラクタ
        public PathBarViewModel(MainViewModel owner, Content content)
        {
            Owner = owner;
           //PathList = new ObservableCollection<Content>();
            PathListViewModel = new ObservableCollection<PathBarItemViewModel>();
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
        private void CreatePathList(Content content)
        {
            Contract.Requires(PathList != null);
            Contract.Requires(!PathList.Any());

            //子要素から親要素に向かってリスト先頭に要素を追加
            PathListViewModel.Insert(0, new PathBarItemViewModel(this, content));
            if(content.Parent != null)
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
            var mainViewModel = sender as MainViewModel;
            if(mainViewModel == null)
            {
                return;
            }
            PathListViewModel.Clear();
            //パスリストの再生成
            CreatePathList(mainViewModel.SelectedItem);
        }

        #endregion
    }
}
