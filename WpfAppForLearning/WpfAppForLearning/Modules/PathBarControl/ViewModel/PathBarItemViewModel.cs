using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAppForLearning.Modules.ContentsTree.Model;
using WpfAppForLearning.Modules.PathBarControl.Command;

namespace WpfAppForLearning.Modules.PathBarControl.ViewModel
{
    public class PathBarItemViewModel
    {
        /// <summary>
        /// オーナービューモデル
        /// </summary>
        public PathBarViewModel OwnerVM { get; set; }

        /// <summary>
        /// 要素に対応するモデル
        /// </summary>
        public Content Model { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 兄弟リスト
        /// </summary>
        public ObservableCollection<PathBarItemViewModel> SiblingList { get; set; }

        /// <summary>
        /// 兄弟リスト表示ボタンを表示するか
        /// </summary>
        public bool IsShownSiblingListDisplayButton { get; set; }

        public ICommand PageTransisitonCommand { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content"></param>
        public PathBarItemViewModel(PathBarViewModel vm, Content content)
        {
            OwnerVM = vm;
            Model = content;
            Name = content.ContentName;

            SiblingListBuilder(content);

            //コマンドのインスタンス化
            PageTransisitonCommand = new PageTransitionCommand(this);
        }

        /// <summary>
        /// 兄弟リストの生成
        /// </summary>
        /// <param name="content"></param>
        private void SiblingListBuilder(Content content)
        {
            SiblingList = new ObservableCollection<PathBarItemViewModel>();

            if (content.Children == null)
            {
                IsShownSiblingListDisplayButton = false;
                return;
            };

            IsShownSiblingListDisplayButton = true;
            foreach (var contentItem in content.Children)
            {
                SiblingList.Add(new PathBarItemViewModel(OwnerVM, contentItem));
            }
        }
    }
}
