using DevelopmentSupport.Common;
using DevelopmentSupport.FileAccessor.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace DevelopmentSupport.FileAccessor.ViewModel
{
    /// <summary>
    /// ファイルアクセスVM
    /// </summary>
    public class FileAccessViewModel : BindableBase
    {

        #region フィールド

        /// <summary>
        /// ファイル情報リスト
        /// </summary>
        protected ObservableCollection<FileInfo> m_FileInfoList;

        /// <summary>
        /// 表示用ファイル情報リスト（フィルタ時用）
        /// </summary>
        protected ObservableCollection<FileInfo> m_DisplayFileInfoList;

        /// <summary>
        /// 選択中のファイル情報
        /// </summary>
        protected FileInfo m_SelectedFileInfo;

        /// <summary>
        /// 拡張子リスト（ファイル情報の更新に追従する）
        /// </summary>
        protected ObservableCollection<Extension> m_ExtensionList;

        /// <summary>
        /// フィルタ中か
        /// </summary>
        protected bool m_IsFiltering = false;

        /// <summary>
        /// ブラウザ情報リスト
        /// </summary>
        protected ObservableCollection<Browser> m_BrowserList;

        /// <summary>
        /// リストに要素が存在するか
        /// </summary>
        protected bool m_HasItem;

        /// <summary>
        /// フィルタ文字列
        /// </summary>
        protected string m_FilterKeyword;

        /// <summary>
        /// フィルタ対象拡張子
        /// </summary>
        protected string m_FilterExtension;

        #endregion

        #region プロパティ

        public ObservableCollection<FileInfo> FileInfoList { get { return m_FileInfoList; } set { SetProperty(ref m_FileInfoList, value); } }
        public ObservableCollection<FileInfo> DisplayFileInfoList { get { return m_DisplayFileInfoList; } set { SetProperty(ref m_DisplayFileInfoList, value); } }
        public FileInfo SelectedFileInfo { get { return m_SelectedFileInfo; } set { SetProperty(ref m_SelectedFileInfo, value); } }
        public ObservableCollection<Extension> ExtensionList { get { return m_ExtensionList; } set { SetProperty(ref m_ExtensionList, value); } }
        public bool IsFiltering { get { return m_IsFiltering; } set { SetProperty(ref m_IsFiltering, value); } }
        public ObservableCollection<Browser> BrowserList { get { return m_BrowserList; } set { SetProperty(ref m_BrowserList, value); } }
        public bool HasItem { get { return m_HasItem; } set { SetProperty(ref m_HasItem, value); } }

        public DelegateCommand ProcessStartCommand { get; protected set; }
        public DelegateCommand ProcessCloseCommand { get; protected set; }
        public DelegateCommand RemoveItemCommand { get; protected set; }
        public DelegateCommand ChangeItemOrderUpperCommand { get; protected set; }
        public DelegateCommand ChangeItemOrderLowerCommand { get; protected set; }
        public DelegateCommand<string> FilterByKeywordCommand { get; protected set; }
        public DelegateCommand<string> FilterByExtensionCommand { get; protected set; }
        public DelegateCommand<Browser> OpenLinkBySelectedAppCommand { get; protected set; }

        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileAccessViewModel()
        {
            // フィルタ情報の初期化
            m_FilterExtension = "(指定なし)";
            m_FilterKeyword = "";

            // ファイル情報リストの初期化
            FileInfoList = new ObservableCollection<FileInfo>();
            HasItem = FileInfoList.Any();
            DisplayFileInfoList = new ObservableCollection<FileInfo>();
            SelectedFileInfo = new FileInfo("");

            // ブラウザ情報の初期化
            BrowserList = new ObservableCollection<Browser>
            {
                new Browser() { Name = "Chrome", Path = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" },
                new Browser() { Name = "InternetExplorer", Path = @"C:\Program Files\internet explorer\iexplore.exe" },
                new Browser() { Name = "FireFox", Path = "" }
            };

            //　拡張子リストの初期化
            ExtensionList = new ObservableCollection<Extension>();
            ExtensionList.Add(new Extension("(指定なし)"));

            // コマンドの初期化
            ProcessCloseCommand = new DelegateCommand(ProcessClose, CanProcessClose);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
            ChangeItemOrderUpperCommand = new DelegateCommand(ChangeItemOrderUpper, CanChangeItemOrderUpper);
            ChangeItemOrderLowerCommand = new DelegateCommand(ChangeItemOrderLower, CanChangeItemOrderLower);
            ProcessStartCommand = new DelegateCommand(ProcessStart, CanProcessStart);
            FilterByKeywordCommand = new DelegateCommand<string>(FilterByKeywordInternal, CanFilterByKeywordInternal);
            FilterByExtensionCommand = new DelegateCommand<string>(FilterByExtension, CanFilterByExtension);
            OpenLinkBySelectedAppCommand = new DelegateCommand<Browser>(OpenLinkBySelectedApp, CanOpenLinkBySelectedApp);
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// プロセスの終了を捕捉する Exited イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Process_Exited(object sender, EventArgs e)
        {
            var proc = (System.Diagnostics.Process)sender;

            var process = DisplayFileInfoList.Where(x => x?.Process?.Id == proc?.Id);
            if (process.Any())
            {
                foreach (var item in process)
                {
                    item.Process = null;
                    item.Processing = false;
                }
            }
            process = FileInfoList.Where(x => x?.Process?.Id == proc?.Id);
            if(!process.Any())
            {
                return;
            }
            foreach (var item in process)
            {    
                item.Process = null;
                item.Processing = false;
            }
        }

        /// <summary>
        /// 指定された文字列でフィルタする
        /// </summary>
        private void FilterInternal()
        {
            var filteredList = FileInfoList.Select(x => x);
            if (m_FilterExtension == "(指定なし)")
            {
                filteredList = filteredList.Where(x => x.FilePath.Contains(m_FilterKeyword));
            }
            else
            {
                filteredList = filteredList.Where(x => Path.GetExtension(x.FilePath) == m_FilterExtension && x.FilePath.Contains(m_FilterKeyword));
            }
            DisplayFileInfoList.Clear();
            foreach (var item in filteredList)
            {
                DisplayFileInfoList.Add(item);
            }
            IsFiltering = true;
        }

        #endregion

        #region 公開サービス

        /// <summary>
        /// リストにファイルを追加する
        /// </summary>
        /// <param name="files">D&Dの結果</param>
        public void AddItems(string[] files)
        {
            var pathList = FileInfoList.Select(x => x.FilePath);
            var DuplicateFilePathList = new List<string>();

            foreach (var s in files)
            {
                if (pathList.Contains(s))
                {
                    DuplicateFilePathList.Add(s);
                    continue;
                }
                var fileInfo = new FileInfo(s);
                FileInfoList.Add(fileInfo);
                SynchronizeDisplayFileList();

                var extension = Path.GetExtension(s);
                if (!ExtensionList.Select(x => x.Name).Contains(extension))
                {
                    Extension addExtension;

                    if (extension == "")
                    {
                        addExtension = new Extension("", "(フォルダ)");
                    }
                    else
                    {
                        addExtension = new Extension(extension);
                    }
                    ExtensionList.Add(addExtension);
                }
            }

            if (DuplicateFilePathList.Any())
            {
                var message = "下記のアイテムは既に登録済みのため登録されませんでした。\n";
                foreach (var item in DuplicateFilePathList)
                {
                    message += " " + item + "\n";
                }
                MessageBox.Show(message);
            }

            HasItem = FileInfoList.Any();
        }

        /// <summary>
        /// 表示用のファイルリストと同期する
        /// </summary>
        public void SynchronizeDisplayFileList()
        {
            DisplayFileInfoList.Clear();
            foreach (var item in FileInfoList)
            {
                DisplayFileInfoList.Add(item);
            }
        }

        /// <summary>
        /// キーワードでフィルタする
        /// </summary>
        /// <param name="keyword">キーワード</param>
        public void FilterByKeyword(string keyword)
        {
            if (!FileInfoList.Any())
            {
                return;
            }

            CanFilterByKeywordInternal(keyword);
        }

        #endregion

        #region コマンド実装

        #region プロセスを開始する

        /// <summary>
        /// プロセスを開始可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanProcessStart()
		{
	        return DisplayFileInfoList.Any() && SelectedFileInfo != null;
        }

        /// <summary>
        /// プロセスを開始する
        /// </summary>
        protected void ProcessStart()
        {
            //Processオブジェクトを作成する
            SelectedFileInfo.Process = new System.Diagnostics.Process();
            //起動する実行ファイルのパスを設定する
            SelectedFileInfo.Process.StartInfo.FileName = SelectedFileInfo.FilePath;
            //コマンドライン引数を指定する
            //起動する。プロセスが起動した時はTrueを返す。

            SelectedFileInfo.Processing = true;
            SelectedFileInfo.Process.EnableRaisingEvents = true;
            //SelectedFileInfo.Process.Exited += new EventHandler(Process_Exited);
            SelectedFileInfo.Process.Start();
        }

        #endregion

        #region プロセスを終了する

        /// <summary>
        /// プロセスを終了可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanProcessClose()
        {
            return SelectedFileInfo != null && SelectedFileInfo.Process != null;
        }

        /// <summary>
        /// プロセスを終了する
        /// </summary>
        protected void ProcessClose()
        {
            SelectedFileInfo.Process.CloseMainWindow();
            SelectedFileInfo.Process.Close();
            SelectedFileInfo.Process = null;
            SelectedFileInfo.Processing = false;
        }

        #endregion

        #region 選択中のリスト要素を削除する

        /// <summary>
        /// 選択中のリスト要素を削除可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanRemoveItem()
        {
            return FileInfoList.Any() && SelectedFileInfo != null;
        }

        /// <summary>
        /// 選択中のリスト要素を削除する
        /// </summary>
        protected void RemoveItem()
        {
            var index = DisplayFileInfoList.IndexOf(SelectedFileInfo);

            FileInfoList.Remove(SelectedFileInfo);
            DisplayFileInfoList.Remove(SelectedFileInfo);

            //選択アイテムの更新
            if (DisplayFileInfoList.Count == 0)
            {
                //表示対象がない
                index = -1;
            }
            else if (DisplayFileInfoList.Count <= index)
            {
                index = DisplayFileInfoList.Count - 1;
            }
            else
            {
                //何もしない
            }

            if (index >= 0)
            {
                DisplayFileInfoList.ElementAt(index).IsSelected = true;
            }

            //フィルタの更新
            var missingKeywordList = new List<Extension>();
            foreach (var item in ExtensionList)
            {
                missingKeywordList.Add(item);
            }
            missingKeywordList.RemoveAt(0);
            foreach (var item in DisplayFileInfoList)
            {
                var extension = Path.GetExtension(item.FilePath);
                var extensionItems = ExtensionList.Where(x => x.Name == extension);

                if (extensionItems.Any())
                {
                    foreach (var extItem in extensionItems)
                    {
                        missingKeywordList.Remove(extItem);
                    }
                }
            }

            // 拡張子リストの更新
            foreach (var item in missingKeywordList)
            {
                ExtensionList.Remove(item);
            }

            HasItem = FileInfoList.Any();
        }

        #endregion

        #region 選択中のリスト要素の順番を前に移動する

        /// <summary>
        /// 選択中のリスト要素の順番を前に移動可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanChangeItemOrderUpper()
        {
            return FileInfoList.Any() && SelectedFileInfo != null && FileInfoList.IndexOf(SelectedFileInfo) > 0 && !IsFiltering;
        }

        /// <summary>
        /// 選択中のリスト要素の順番を前に移動する
        /// </summary>
        protected void ChangeItemOrderUpper()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            SelectedFileInfo = null;
            FileInfoList.Move(index, index - 1);
            SynchronizeDisplayFileList();
            SelectedFileInfo = DisplayFileInfoList[index - 1];
        }

        #endregion

        #region 選択中のリスト要素の順番を前に移動する

        /// <summary>
        /// 選択中のリスト要素の順番を後に移動可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanChangeItemOrderLower()
        {
            return FileInfoList.Any() && SelectedFileInfo != null && FileInfoList.Count > FileInfoList.IndexOf(SelectedFileInfo) + 1 && !IsFiltering;
        }

        /// <summary>
        /// 選択中のリスト要素の順番を前に移動する
        /// </summary>
        protected void ChangeItemOrderLower()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            SelectedFileInfo = null;
            FileInfoList.Move(index, index + 1);
            SynchronizeDisplayFileList();
            SelectedFileInfo = DisplayFileInfoList[index + 1];
        }

        #endregion

        #region リンクを指定したブラウザで開く

        /// <summary>
        /// リンクファイルを指定したブラウザで開く事が可能か
        /// </summary>
        /// <param name="browser">指定されたブラウザ情報</param>
        /// <returns></returns>
        protected bool CanOpenLinkBySelectedApp(Browser browser)
        {
            return (browser != null && SelectedFileInfo.IsLink && File.Exists(browser.Path));
        }

        /// <summary>
        /// リンクファイルを指定したブラウザで開く
        /// </summary>
        /// <param name="browser">指定されたブラウザ情報</param>
        protected void OpenLinkBySelectedApp(Browser browser)
        {
            var textLines = File.ReadAllLines(SelectedFileInfo.FilePath);
            string urlLine = "";
            foreach (var line in textLines)
            {
                if (line.StartsWith("URL="))
                {
                    urlLine = line;
                    break;
                }
            }

            if (urlLine == "")
            {
                return;
            }

            SelectedFileInfo.StartProcessBySelectedApp(browser.Path, urlLine.Substring(4));

        }

        #endregion

        #region フィルタ

        /// <summary>
        /// キーワードフィルタを適用できるか
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool CanFilterByKeywordInternal(string keyword)
        {
            return true;
        }

        /// <summary>
        /// キーワードフィルタを適用する
        /// </summary>
        /// <param name="keyword"></param>
        private void FilterByKeywordInternal(string keyword)
        {
            m_FilterKeyword = keyword;
            FilterInternal();
        }

        private bool CanFilterByExtension(string extension)
        {
            return true;
        }

        /// <summary>
        /// 拡張子でフィルタする
        /// </summary>
        /// <param name="extnsion"></param>
        private void FilterByExtension(string extension)
        {
            m_FilterExtension = extension;
            FilterInternal();
        }

        #endregion

        #endregion

    }

    /// <summary>
    /// ファイル情報
    /// </summary>
    public class FileInfo : BindableBase
    {
        #region フィールド

        /// <summary>
        /// ファイルパス
        /// </summary>
        protected string m_FilePath;

        /// <summary>
        /// ファイル名
        /// </summary>
        protected string m_FileName;

        /// <summary>
        /// アイコン
        /// </summary>
        protected BitmapSource m_Icon;

        /// <summary>
        /// ファイルを開くプロセス
        /// </summary>
        protected Process m_Process = null;

        /// <summary>
        /// プロセス起動中か
        /// </summary>
        protected bool m_Processing = false;

        /// <summary>
        /// プロセス情報
        /// </summary>
        protected Process m_SettingProcess = null;

        /// <summary>
        /// リスト要素として選択中か
        /// </summary>
        protected bool m_IsSelected = false;

        /// <summary>
        /// リンクファイルか
        /// </summary>
        protected bool m_IsLink = false;

        #endregion

        #region プロパティ

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath { get { return m_FilePath; } set { SetProperty(ref m_FilePath, value); } }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get { return m_FileName; } set { SetProperty(ref m_FileName, value); } }

        /// <summary>
        /// アイコン
        /// </summary>
        public BitmapSource Icon { get { return m_Icon; } set { SetProperty(ref m_Icon, value); } }

        /// <summary>
        /// ファイルを開くプロセス
        /// </summary>
        public Process Process { get { return m_Process; } set { SetProperty(ref m_Process, value); } }

        /// <summary>
        /// プロセス起動中か
        /// </summary>
        public bool Processing { get { return m_Processing; } set { SetProperty(ref m_Processing, value); } }

        /// <summary>
        /// プロセス情報
        /// </summary>
        public Process SettingProcess { get { return m_SettingProcess; } set { SetProperty(ref m_SettingProcess, value); } }

        /// <summary>
        /// リスト要素として選択中か
        /// </summary>
        public bool IsSelected { get { return m_IsSelected; } set { SetProperty(ref m_IsSelected, value); } }

        /// <summary>
        /// リンクファイルか
        /// </summary>
        public bool IsLink { get { return m_IsLink; } set { SetProperty(ref m_IsLink, value); } }

        /// <summary>
        /// パステキストをコピーするコマンド
        /// </summary>
        public DelegateCommand TextCopyCommand { get; protected set; }

        /// <summary>
        /// ファイルが存在するパスを開くコマンド
        /// </summary>
        public DelegateCommand OpenFilePlacementFolderCommand { get; protected set; }

        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public FileInfo(string filePath)
        {
            FilePath = filePath;
            FileName = System.IO.Path.GetFileName(filePath);
            IsLink = Path.GetExtension(filePath) == ".url";
            if (File.Exists(filePath))
            {
                var icon = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
                Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else if (Directory.Exists(filePath))
            {
                var shinfo = new SHFILEINFO();
                var hImgSmall = Win32.SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
                var icon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
                Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }

            TextCopyCommand = new DelegateCommand(TextCopy, CanTextCopy);
            OpenFilePlacementFolderCommand = new DelegateCommand(OpenFilePlacementFolder, CanOpenFilePlacementFolder);

        }

        #endregion

        #region コマンド実装

        #region ファイルパスをクリップボードにコピー

        /// <summary>
        /// ファイルパスをクリップボードにコピー可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanTextCopy()
        {
            return true;
        }

        /// <summary>
        /// ファイルパスをクリップボードにコピー
        /// </summary>
        protected void TextCopy()
        {
            Clipboard.SetData(DataFormats.Text, FilePath);
        }

        #endregion

        #region ファイルのあるフォルダを開く

        /// <summary>
        /// ファイルのあるフォルダを開くことが可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanOpenFilePlacementFolder()
        {
            return true;
        }

        /// <summary>
        /// ファイルのあるフォルダを開く
        /// </summary>
        protected void OpenFilePlacementFolder()
        {
            System.Diagnostics.Process.Start("EXPLORER.EXE", @"/select,""" + FilePath + @"""");
        }

        #endregion

        #endregion

        #region 公開サービス

        /// <summary>
        /// 指定したアプリケーションでファイルを開く
        /// </summary>
        /// <param name="path">アプリケーションパス</param>
        /// <param name="arguments">引数情報</param>
        /// <returns>プロセス起動に成功したか</returns>
        public bool StartProcessBySelectedApp(string path, string arguments)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            //Processオブジェクトを作成する
            SettingProcess = new System.Diagnostics.Process();
            //起動する実行ファイルのパスを設定する
            SettingProcess.StartInfo.FileName = path;
            SettingProcess.StartInfo.Arguments = arguments;
            //コマンドライン引数を指定する
            //起動する。プロセスが起動した時はTrueを返す。
            return SettingProcess.Start();
        }

        #endregion

    }

    /// <summary>
    /// ブラウザ情報クラス
    /// </summary>
    public class Browser : BindableBase
    {
        #region フィールド

        /// <summary>
        /// ブラウザ名
        /// </summary>
        private string m_Name;
        /// <summary>
        /// ブラウザのアプリケーションパス
        /// </summary>
        private string m_Path;

        #endregion

        #region プロパティ
        /// <summary>
        /// ブラウザ名
        /// </summary>
        public string Name { get { return m_Name; } set { SetProperty(ref m_Name, value); } }

        /// <summary>
        /// ブラウザのアプリケーションパス
        /// </summary>
        public string Path { get { return m_Path; } set { SetProperty(ref m_Path, value); } }

        #endregion
    }

    /// <summary>
    /// 拡張子クラス
    /// </summary>
    public class Extension : BindableBase
	{
		#region フィールド

		/// <summary>
		/// 拡張子を表す文字列
		/// </summary>
		private string m_Name;

		/// <summary>
		/// 拡張子に対応する表示用文字列
		/// </summary>
		private string m_DisplayName;

        #endregion

        #region プロパティ

        /// <summary>
        /// 拡張子を表す文字列
        /// </summary>
        public string Name { get { return m_Name; } set { SetProperty(ref m_Name, value); } }
        /// <summary>
        /// 拡張子に対応する表示用文字列
        /// </summary>
		public string DisplayName { get { return m_DisplayName; } set { SetProperty(ref m_DisplayName, value); } }

		#endregion

		#region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">拡張子名</param>
		public Extension(string name)
		{
			m_Name = name;
            // 表示用に拡張子名と同じ文字列を設定する
			m_DisplayName = m_Name;
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">拡張子名</param>
        /// <param name="displayName">表示用拡張子名</param>
		public Extension(string name, string displayName)
		{
			m_Name = name;
			m_DisplayName = displayName;
		}

		#endregion
	}
}
