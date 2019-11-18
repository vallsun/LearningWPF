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
using WpfAppForLearning.Properties;
using WpfAppForLearning.Modules.StartControl;
using WpfAppForLearning.Modules.ProgressBar;
using WpfAppForLearning.Modules.KeyboardNavigation;
using WpfAppForLearning.Modules.DragDropControl;
using WpfAppForLearning.Modules.TextBoxControl;

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

        /// <summary>
        /// 選択中のアイテムが変更された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != "SelectedItem")
            {
                return;
            }

            PathBar.OnSelectedItemChanged(sender, e);

            //ContentViewModel = null;
            //選択されたコンテンツに対応するVMを設定する
            //switch-case文ではcaseに定数値以外を設定できないため、if文で実装する
            if(SelectedItem.ContentName == "コンテンツ")
            {
                ContentViewModel = new StartControlViewModel();
            }
            else if (SelectedItem.ContentName == Strings.ContentName_CustomControl)
            {
                ContentViewModel = new CustomControlViewModel();
            }
            else if (SelectedItem.ContentName == Strings.ContentName_ProgressBar)
            {
                ContentViewModel = new ProgressBarViewModel(); 
            }
            else if(SelectedItem.ContentName == Strings.ContentName_KeyboardNavigation)
            {
                ContentViewModel = new KeyboardNavigationViewModel();
            }
            else if (SelectedItem.ContentName == Strings.ContentName_DragDropControl)
            {
                ContentViewModel = new DragDropControlViewModel();
            }
            else if(SelectedItem.ContentName == "TextBox")
            {
                ContentViewModel = new TextBoxControlViewModel();
            }

            else
            {
                //未実装コンテンツ用のVMを設定
                ContentViewModel = new NotImplementationViewModel();
            }
        }

        /// <summary>
        /// プロパティ変更通知
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
