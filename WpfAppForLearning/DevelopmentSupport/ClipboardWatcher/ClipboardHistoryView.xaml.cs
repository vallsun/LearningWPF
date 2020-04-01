using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DevelopmentSupport.ClipboardWatcher
{
    /// <summary>
    /// ClipboardHistoryView.xaml の相互作用ロジック
    /// </summary>
    public partial class ClipboardHistoryView : UserControl
    {

        #region フィールド

        /// <summary>
        /// クリップボード監視機能
        /// </summary>
        private ClipboardWatcher clipboardWatcher = null;

        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClipboardHistoryView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ClipboardHistoryView()
        {
            clipboardWatcher.Dispose();
        }

        #endregion

        /// <summary>
        /// クリップボード更新時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clipboardWatcher_DrawClipboard(object sender, System.EventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                return;
            }
            var vm = DataContext as ClipboardHistoryViewModel;

            if (vm == null)
            {
                return;
            }

            try
            {
                var text = Clipboard.GetText();
                if (vm.ItemList.Contains(text))
                {
                    // 重複要素の場合、過去の要素を削除し、最新の要素として追加
                    vm.ItemList.Remove(text);
                }
                vm.ItemList.Add(text);
            }
            catch (System.Runtime.InteropServices.COMException)
            { 
                // NOP
                // http://shen7113.blog.fc2.com/blog-entry-28.html
            }
        }

        /// <summary>
        /// ロード後のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClipboardHistoryViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.MainWindow == null)
            {
                return;
            }
            clipboardWatcher = new ClipboardWatcher(new System.Windows.Interop.WindowInteropHelper(Application.Current.MainWindow).Handle);
            clipboardWatcher.DrawClipboard += clipboardWatcher_DrawClipboard;
        }
    }
}
