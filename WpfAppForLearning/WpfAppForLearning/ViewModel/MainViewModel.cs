using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppForLearning.Modules.ContentsTree.Model;
using WpfAppForLearning.Modules.PathBarControl.ViewModel;

namespace WpfAppForLearning.ViewModel
{
    public class MainViewModel :INotifyPropertyChanged
    {
        /// <summary>
        /// コンテンツ
        /// </summary>
        public Contents ContentsTree { get; set; }
        /// <summary>
        /// パスリストのビューモデル
        /// </summary>
        public PathBarViewModel PathBar { get; set; }

        private static readonly PropertyChangedEventArgs SelectedItemPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedItem));

        private Content m_SelectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

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


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel()  
        {
            //コンテンツの生成
            ContentsTree = new Contents();
            PathBar = new PathBarViewModel(this, SelectedItem);
            SelectedItem = (Content)ContentsTree.SelectedItem;

            //イベントハンドラの接続
            PropertyChanged += PathBar.OnSelectedItemChanged;
        }
    }
}
