using DevelopmentSupport.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Setting
{
    public class SettingDialogViewModel : BindableBase
    {

        private ObservableCollection<Setting> m_SettingTree;
        private object m_ContentViewModel;
        private Setting m_SelectedItem;

        public ObservableCollection<Setting> SettingTree { get { return m_SettingTree; } set { SetProperty(ref m_SettingTree, value); } }
        public object ContentViewModel { get { return m_ContentViewModel; } set { SetProperty(ref m_ContentViewModel, value); } }
        public Setting SelectedItem { get { return m_SelectedItem; } set { SetProperty(ref m_SelectedItem, value); } }

        public SettingDialogViewModel()
        {
            CreateSettingtree();
            //イベントハンドラの接続
            PropertyChanged += OnSelectedItemChanged;
        }

        private void CreateSettingtree()
        {
            m_SettingTree = new ObservableCollection<Setting>()
            {
                new Setting() { Name = "基本設定" },
                new Setting() { Name = "ブラウザ設定" }
            };
        }

        /// <summary>
        /// 選択中のアイテムが変更された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "SelectedItem")
            {
                return;
            }

            //ContentViewModel = null;
            //選択されたコンテンツに対応するVMを設定する
            //switch-case文ではcaseに定数値以外を設定できないため、if文で実装する
            if (SelectedItem.Name == "ブラウザ設定")
            {
                ContentViewModel = new BrowserSettingViewModel();
            }
            else
            {
                //未実装コンテンツ用のVMを設定
                ContentViewModel = new NotImplementationViewModel();
            }
        }
    }
}
