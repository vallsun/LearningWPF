﻿using DevelopmentSupport.Common;
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

namespace DevelopmentSupport.FileAccessor
{
    public class FileAccessViewModel : BindableBase
    {

        #region フィールド

        protected ObservableCollection<FileInfo> m_FileInfoList;
        protected ObservableCollection<FileInfo> m_DisplayFileInfoList;
        protected FileInfo m_SelectedFileInfo;
        protected ObservableCollection<Extension> m_ExtensionList;
        protected bool m_IsFiltering = false;
        protected ObservableCollection<Browser> m_BrowserList;
        protected bool m_HasItem;
        protected string m_FilterKeyword;
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

        public FileAccessViewModel()
        {
            m_FilterExtension = "(指定なし)";
            m_FilterKeyword = "";
            FileInfoList = new ObservableCollection<FileInfo>();
            HasItem = FileInfoList.Any();
            DisplayFileInfoList = new ObservableCollection<FileInfo>();
            SelectedFileInfo = new FileInfo("");
            BrowserList = new ObservableCollection<Browser>
            {
                new Browser() { Name = "Chrome", Path = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" },
                new Browser() { Name = "InternetExplorer", Path = @"C:\Program Files\internet explorer\iexplore.exe" },
                new Browser() { Name = "FireFox", Path = "" }
            };
            ExtensionList = new ObservableCollection<Extension>();
            ExtensionList.Add(new Extension("(指定なし)"));

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

        // プロセスの終了を捕捉する Exited イベントハンドラ
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
        /// フィルタする
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

        #region コマンド実装

        protected bool CanProcessClose()
        {
            return SelectedFileInfo != null && SelectedFileInfo.Process != null;
        }

        protected void ProcessClose()
        {
            SelectedFileInfo.Process.CloseMainWindow();
            SelectedFileInfo.Process.Close();
            SelectedFileInfo.Process = null;
            SelectedFileInfo.Processing = false;
        }

        protected bool CanRemoveItem()
        {
            return FileInfoList.Any() && SelectedFileInfo != null;
        }

        protected void RemoveItem()
        {
            var index = DisplayFileInfoList.IndexOf(SelectedFileInfo);

            FileInfoList.Remove(SelectedFileInfo);
            DisplayFileInfoList.Remove(SelectedFileInfo);

            //選択アイテムの更新
            if(DisplayFileInfoList.Count == 0)
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

            if(index >= 0)
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

            foreach (var item in missingKeywordList)
            {
                ExtensionList.Remove(item);
            }

            HasItem = FileInfoList.Any();
        }

        protected bool CanChangeItemOrderUpper()
        {
            return FileInfoList.Any() && SelectedFileInfo != null && FileInfoList.IndexOf(SelectedFileInfo) > 0 && !IsFiltering;
        }

        protected void ChangeItemOrderUpper()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            SelectedFileInfo = null;
            FileInfoList.Move(index, index - 1);
            SynchronizeDisplayFileList();
            SelectedFileInfo = DisplayFileInfoList[index - 1];
        }

        protected bool CanChangeItemOrderLower()
        {
            return FileInfoList.Any() && SelectedFileInfo != null && FileInfoList.Count > FileInfoList.IndexOf(SelectedFileInfo) + 1 && !IsFiltering;
        }

        protected void ChangeItemOrderLower()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            SelectedFileInfo = null;
            FileInfoList.Move(index, index + 1);
            SynchronizeDisplayFileList();
            SelectedFileInfo = DisplayFileInfoList[index + 1];
        }

        #region リンクをアプリケーションを指定して開く

        protected bool CanOpenLinkBySelectedApp(Browser browser)
        {
            return (browser != null && SelectedFileInfo.IsLink && File.Exists(browser.Path));
        }

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

        #endregion

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

        /// <summary>
        /// プロセスを開始する
        /// </summary>
        public void ExecuteProcessStart()
        {
            if (!CanProcessStart())
            {
                return;
            }

            ProcessStart();
        }

        #endregion

        #region コマンド

        #region プロセスを開始する

        /// <summary>
        /// プロセスを開始可能か
        /// </summary>
        /// <returns></returns>
        protected bool CanProcessStart()
        {
            if (SelectedFileInfo == null)
            {
                return false;
            }
            return DisplayFileInfoList.Any() && SelectedFileInfo.Process == null;
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
            SelectedFileInfo.Process.Exited += new EventHandler(Process_Exited);
            SelectedFileInfo.Process.Start();
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

    public class FileInfo : BindableBase
    {
        #region フィールド

        protected string m_FilePath;
        protected string m_FileName;
        protected BitmapSource m_Icon;
        protected Process m_Process = null;
        protected bool m_Processing = false;
        protected Process m_SettingProcess = null;
        protected bool m_IsSelected = false;
        protected bool m_IsLink = false;

        #endregion

        #region プロパティ

        public string FilePath { get { return m_FilePath; } set { SetProperty(ref m_FilePath, value); } }
        public string FileName { get { return m_FileName; } set { SetProperty(ref m_FileName, value); } }
        public BitmapSource Icon { get { return m_Icon; } set { SetProperty(ref m_Icon, value); } }
        public Process Process { get { return m_Process; } set { SetProperty(ref m_Process, value); } }
        public bool Processing { get { return m_Processing; } set { SetProperty(ref m_Processing, value); } }
        public Process SettingProcess { get { return m_SettingProcess; } set { SetProperty(ref m_SettingProcess, value); } }
        public bool IsSelected { get { return m_IsSelected; } set { SetProperty(ref m_IsSelected, value); } }
        public bool IsLink { get { return m_IsLink; } set { SetProperty(ref m_IsLink, value); } }

        public DelegateCommand TextCopyCommand { get; protected set; }
        public DelegateCommand OpenFilePlacementFolderCommand { get; protected set; }

        #endregion

        #region 構築・消滅

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

        protected bool CanTextCopy()
        {
            return true;
        }

        protected void TextCopy()
        {
            Clipboard.SetData(DataFormats.Text, FilePath);
        }

        #endregion

        #region ファイルのあるフォルダを開く

        protected bool CanOpenFilePlacementFolder()
        {
            return true;
        }

        protected void OpenFilePlacementFolder()
        {
            System.Diagnostics.Process.Start("EXPLORER.EXE", @"/select,""" + FilePath + @"""");
        }

        #endregion

        #endregion

        #region 公開サービス

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

    public class Browser : BindableBase
    {
        #region フィールド

        private string m_Name;
        private string m_Path;

        #endregion

        #region プロパティ

        public string Name { get { return m_Name; } set { SetProperty(ref m_Name, value); } }
        public string Path { get { return m_Path; } set { SetProperty(ref m_Path, value); } }

        #endregion
    }

    public class Extension : BindableBase
	{
		#region フィールド

		/// <summary>
		/// 拡張子を表す文字列
		/// </summary>
		private string m_Name;

		/// <summary>
		/// 拡張子に対応する文字列
		/// </summary>
		private string m_DisplayName;

		#endregion

		#region プロパティ

		public string Name { get { return m_Name; } set { SetProperty(ref m_Name, value); } }
		public string DisplayName { get { return m_DisplayName; } set { SetProperty(ref m_DisplayName, value); } }

		#endregion

		#region 構築・消滅

		public Extension(string name)
		{
			m_Name = name;
			m_DisplayName = m_Name;
		}

		public Extension(string name, string displayName)
		{
			m_Name = name;
			m_DisplayName = displayName;
		}

		#endregion
	}
}
