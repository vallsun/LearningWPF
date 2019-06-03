using DevelopmentSupport.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DevelopmentSupport.FileAccessor
{
    public class FileAccessViewModel :BindableBase
    {

        protected ObservableCollection<FileInfo> m_FileInfoList;
        protected ObservableCollection<FileInfo> m_DisplayFileInfoList;
        protected FileInfo m_SelectedFileInfo;
        protected ObservableCollection<string> m_ExtensionList;
        protected bool m_IsFiltering = false;
        protected ObservableCollection<Browser> m_BrowserList;

        public ObservableCollection<FileInfo> FileInfoList { get { return m_FileInfoList; } set { SetProperty(ref m_FileInfoList, value); } }
        public ObservableCollection<FileInfo> DisplayFileInfoList { get { return m_DisplayFileInfoList; } set { SetProperty(ref m_DisplayFileInfoList, value); } }
        public FileInfo SelectedFileInfo { get { return m_SelectedFileInfo; } set { SetProperty(ref m_SelectedFileInfo, value); } }
        public ObservableCollection<string> ExtensionList { get { return m_ExtensionList; } set { SetProperty(ref m_ExtensionList, value); } }
        public bool IsFiltering { get { return m_IsFiltering; } set { SetProperty(ref m_IsFiltering, value); } }
        public ObservableCollection<Browser> BrowserList { get { return m_BrowserList; } set { SetProperty(ref m_BrowserList, value); } }

        public DelegateCommand ProcessStartCommand { get; protected set; }
        public DelegateCommand ProcessCloseCommand { get; protected set; }
        public DelegateCommand RemoveItemCommand { get; protected set; }
        public DelegateCommand ChangeItemOrderUpperCommand { get; protected set; }
        public DelegateCommand ChangeItemOrderLowerCommand { get; protected set; }
        public DelegateCommand TextCopyCommand { get; protected set; }
        public DelegateCommand OpenFilePlacementFolderCommand { get; protected set; }
        public DelegateCommand<Browser> OpenLinkBySelectedAppCommand { get; protected set; }

        public FileAccessViewModel()
        {
            FileInfoList = new ObservableCollection<FileInfo>();
            DisplayFileInfoList = new ObservableCollection<FileInfo>();
            SelectedFileInfo = new FileInfo();
            ExtensionList = new ObservableCollection<string>
            {
                "(指定なし)"
            };
            BrowserList = new ObservableCollection<Browser>
            {
                new Browser() { Name = "Chrome", Path = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" },
                new Browser() { Name = "InternetExplorer", Path = @"C:\Program Files\internet explorer\iexplore.exe" },
                new Browser() { Name = "FireFox", Path = "" }
            };

            ProcessCloseCommand = new DelegateCommand(ProcessClose, CanProcessClose);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
            ChangeItemOrderUpperCommand = new DelegateCommand(ChangeItemOrderUpper, CanChangeItemOrderUpper);
            ChangeItemOrderLowerCommand = new DelegateCommand(ChangeItemOrderLower, CanChangeItemOrderLower);
            ProcessStartCommand = new DelegateCommand(ProcessStart, CanProcessStart);
            TextCopyCommand = new DelegateCommand(TextCopy, CanTextCopy);
            OpenFilePlacementFolderCommand = new DelegateCommand(OpenFilePlacementFolder, CanOpenFilePlacementFolder);
            OpenLinkBySelectedAppCommand = new DelegateCommand<Browser>(OpenLinkBySelectedApp, CanOpenLinkBySelectedApp);
        }

        // ListBoxのアイテムをダブルクリックされたら呼ばれるメソッド
        public void Execute()
        {
            if (SelectedFileInfo == null)
            {
                return;
            }
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
            var missingKeywordList = new List<string>();
            foreach (var item in ExtensionList)
            {
                missingKeywordList.Add(item);
            }
            missingKeywordList.RemoveAt(0);
            foreach (var item in DisplayFileInfoList)
            {
                var extension = Path.GetExtension(item.FilePath);
                if (ExtensionList.Contains(extension))
                {
                    missingKeywordList.Remove(extension);
                }
            }

            foreach (var item in missingKeywordList)
            {
                ExtensionList.Remove(item);
            }
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
        protected bool CanProcessStart()
        {
            if (SelectedFileInfo == null)
            {
                return false;
            }
            return DisplayFileInfoList.Any() && SelectedFileInfo.Process == null;
        }
        protected void ProcessStart()
        {
            Execute();
        }

        protected bool StartProcessBySelectedApp(string path, string arguments)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            //Processオブジェクトを作成する
            SelectedFileInfo.SettingProcess = new System.Diagnostics.Process();
            //起動する実行ファイルのパスを設定する
            SelectedFileInfo.SettingProcess.StartInfo.FileName = path;
            SelectedFileInfo.SettingProcess.StartInfo.Arguments = arguments;
            //コマンドライン引数を指定する
            //起動する。プロセスが起動した時はTrueを返す。
            return SelectedFileInfo.SettingProcess.Start();
        }

        #region 公開サービス

        public void SynchronizeDisplayFileList()
        {
            DisplayFileInfoList.Clear();
            foreach (var item in FileInfoList)
            {
                DisplayFileInfoList.Add(item);
            }
        }

        #endregion

        #region コマンド

        protected bool CanTextCopy()
        {
            return SelectedFileInfo != null;
        }

        protected void TextCopy()
        {
            Clipboard.SetData(DataFormats.Text, SelectedFileInfo.FilePath);
        }

        protected bool CanOpenFilePlacementFolder()
        {
            return SelectedFileInfo != null;
        }

        protected void OpenFilePlacementFolder()
        {
            System.Diagnostics.Process.Start("EXPLORER.EXE", @"/select,""" + SelectedFileInfo.FilePath + @"""");
        }

        protected bool CanOpenLinkBySelectedApp(Browser browser)
        {
            return (browser != null && SelectedFileInfo != null && SelectedFileInfo.IsLink && File.Exists(browser.Path));
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

            if(urlLine == "")
            {
                return;
            }

            StartProcessBySelectedApp(browser.Path, urlLine.Substring(4));

        }



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

        #endregion

        #region プロパティ

        public string FilePath { get { return m_FilePath; } set { SetProperty(ref m_FilePath, value); } }
        public string FileName { get { return m_FileName; } set { SetProperty(ref m_FileName, value); } }
        public BitmapSource Icon { get { return m_Icon; } set { SetProperty(ref m_Icon, value); } }
        public Process Process { get { return m_Process; } set { SetProperty(ref m_Process, value); } }
        public bool Processing { get { return m_Processing; } set { SetProperty(ref m_Processing, value); } }
        public Process SettingProcess { get { return m_SettingProcess; } set { SetProperty(ref m_SettingProcess, value); } }
        public bool IsSelected { get { return m_IsSelected; } set { SetProperty(ref m_IsSelected, value); } }

        public bool IsLink { get { return Path.GetExtension(FilePath) == ".url";  } }

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
}
