using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppForLearning.Modules.Common;
using WpfAppForLearning.Modules.ContentsTree.Model;
using WpfAppForLearning.Modules.CustomControl;
using WpfAppForLearning.Modules.PathBarControl.ViewModel;
using WpfAppForLearning.Modules.StartControl;

namespace WpfAppForLearning.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region フィールド

        /// <summary>
        /// 選択中のアイテム
        /// </summary>
        private Content m_SelectedItem;

        /// <summary>
        /// 表示対象コンテンツのビューモデル
        /// </summary>
        private object m_ContentViewModel;

        #endregion

        #region プロパティ

        /// <summary>
        /// コンテンツ
        /// </summary>
        public Contents ContentsTree { get; set; }
        /// <summary>
        /// パスリストのビューモデル
        /// </summary>
        public PathBarViewModel PathBar { get; set; }

        /// <summary>
        /// 表示対象コンテンツのビューモデル
        /// </summary>
        public object ContentViewModel
        { get
            {
                return m_ContentViewModel;
            }
            set
            {
                if (m_ContentViewModel == value)
                {
                    return;
                }
                m_ContentViewModel = value;
                OnPropertyChanged("ContentViewModel");
            }
        }

        /// <summary>
        /// 選択中のアイテム
        /// </summary>
        public Content SelectedItem
        {
            get { return this.m_SelectedItem; }
            set
            {
                if (this.m_SelectedItem == value) { return; }
                this.m_SelectedItem = value;
                this.PropertyChanged?.Invoke(this, SelectedItemPropertyChangedEventArgs);
            }
        }

        #endregion

        private static readonly PropertyChangedEventArgs SelectedItemPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedItem));

        public event PropertyChangedEventHandler PropertyChanged;

        


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel()  
        {
            //コンテンツの生成
            ContentsTree = new Contents();
            PathBar = new PathBarViewModel(this, SelectedItem);
            //ContentViewModel = new StartControlViewModel();
            ContentViewModel = new CustomControlViewModel();

            //イベントハンドラの接続
            PropertyChanged += OnSelectedItemChanged;

            SelectedItem = ContentsTree.ContentsTree.First<Content>();
        }

        #region イベントハンドラ

        public void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != "SelectedItem")
            {
                return;
            }

            PathBar.OnSelectedItemChanged(sender, e);

            ContentViewModel = null;
            switch (SelectedItem.ContentName)
            {
                case "コンテンツ":
                    ContentViewModel = new StartControlViewModel();
                    break;
                case "カスタムコントロール":
                    ContentViewModel = new CustomControlViewModel();
                    break;
                default:
                    ContentViewModel = new NotImplementationViewModel();
                    break;
            }
        }

        /// <summary>
        /// プロパティ変更通知
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

    }
}
