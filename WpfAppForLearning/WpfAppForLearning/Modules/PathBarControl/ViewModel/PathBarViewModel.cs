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
    public class PathBarViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Content> PathList { get; set; }

        public ObservableCollection<PathBarItemViewModel> PathListViewModel { get; set; }

        public object Owner { get; set; }

        //コンストラクタ
        public PathBarViewModel(object owner, Content content)
        {
            Owner = owner;
           //PathList = new ObservableCollection<Content>();
            PathListViewModel = new ObservableCollection<PathBarItemViewModel>();
            if (content != null)
            {
                CreatePathList(content);
            }
        }

        /// <summary>
        /// パスリストを生成
        /// </summary>
        /// <param name="content"></param>
        private void CreatePathList(Content content)
        {
            Contract.Requires(PathList != null);
            Contract.Requires(!PathList.Any());

            

            //子要素から親要素に向かってリスト先頭に要素を追加
            PathListViewModel.Insert(0, new PathBarItemViewModel(content));
            if(content.Parent != null)
            {
                CreatePathList(content.Parent);
            };

            /*
            //子要素から親に向かってリストの先頭に追加
            PathList.Insert(0, content);
            if(content.Parent != null)
            {
                CreatePathList(content.Parent);
            }
            */
        }

        #region イベントハンドラ

        public void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            var mainViewModel = sender as MainViewModel;
            if(mainViewModel == null)
            {
                return;
            }
            //PathList.Clear();
            PathListViewModel.Clear();
            CreatePathList(mainViewModel.SelectedItem);
        }

        #endregion
    }
}
