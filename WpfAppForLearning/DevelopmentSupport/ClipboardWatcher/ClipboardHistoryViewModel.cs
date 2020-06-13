using DevelopmentCommon.Common;
using DevelopmentSupport.Common;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DevelopmentSupport.ClipboardWatcher
{
    /// <summary>
    /// クリップボードの履歴を表示するVM
    /// </summary>
    public class ClipboardHistoryViewModel : BindableBase
    {

        #region フィールド

        /// <summary>
        /// テキストリスト
        /// </summary>
        public ObservableCollection<string> m_ItemList = new ObservableCollection<string>();

        /// <summary>
        /// 選択中のリスト要素
        /// </summary>
        public string m_SelectedItem = string.Empty;

        #endregion

        #region プロパティ

        /// <summary>
        /// テキストリスト
        /// </summary>
        public ObservableCollection<string> ItemList { get { return m_ItemList; } set { SetProperty(ref m_ItemList, value); } }

        /// <summary>
        /// 選択中の要素
        /// </summary>
        public string SelectedItem { get { return m_SelectedItem; } set { SetProperty(ref m_SelectedItem, value); } }

        #endregion

        /// <summary>
        /// テキストをコピーするコマンド
        /// </summary>
        public DelegateCommand TextCopyCommand { get; protected set; }

        /// <summary>
        /// リスト要素を削除するコマンド
        /// </summary>
        public DelegateCommand RemoveItemCommand { get; protected set; }

        /// <summary>
        /// すべてのリスト要素を削除するコマンド
        /// </summary>
        public DelegateCommand RemoveAllCommand { get; protected set; }


        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClipboardHistoryViewModel()
        {
            // コマンドの初期化
            TextCopyCommand = new DelegateCommand(TextCopy, CanTextCopy);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
            RemoveAllCommand = new DelegateCommand(RemoveAll, CanRemoveAll);
        }

        #endregion

        #region コマンド実装

        #region 選択中のリスト要素を削除する

        /// <summary>
        /// 選択中のリスト要素を削除可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanRemoveItem()
        {
            return ItemList.Any() && SelectedItem != null;
        }

        /// <summary>
        /// 選択中のリスト要素を削除する
        /// </summary>
        protected void RemoveItem()
        {
            var index = ItemList.IndexOf(SelectedItem);

            ItemList.Remove(SelectedItem);

            //選択アイテムの更新
            if (ItemList.Count == 0)
            {
                //表示対象がない
                index = -1;
            }
            else if (ItemList.Count <= index)
            {
                index = ItemList.Count - 1;
            }
            else
            {
                //何もしない
            }
        }

        #endregion

        #region すべてのリスト要素を削除する

        /// <summary>
        /// すべてのリスト要素を削除可能か
        /// </summary>
        /// <returns></returns>
        private bool CanRemoveAll()
        {
            return ItemList.Any();
        }

        /// <summary>
        /// すべてのリスト要素を削除する
        /// </summary>
        private void RemoveAll()
        {
            ItemList.Clear();
        }

        #endregion

        #region テキストをクリップボードにコピー

        /// <summary>
        /// テキストをクリップボードにコピー可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanTextCopy()
        {
            return true;
        }

        /// <summary>
        /// テキストをクリップボードにコピー
        /// </summary>
        protected void TextCopy()
        {
            Clipboard.SetData(DataFormats.Text, SelectedItem);
        }

        #endregion

        #endregion
    }
}
