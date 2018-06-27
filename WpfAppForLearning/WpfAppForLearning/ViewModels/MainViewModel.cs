using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppForLearning.Models;

namespace WpfAppForLearning.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// コンテンツ一覧
        /// </summary>
        public List<LearningContents> LearningContentsList;

        /// <summary>
        /// 現在選択中の要素
        /// </summary>
        public LearningContents SelectedItem { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
