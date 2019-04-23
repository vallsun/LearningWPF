using DevelopmentSupport.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ObservableCollection<FileInfo> FileInfoList { get { return m_FileInfoList; } set { SetProperty(ref m_FileInfoList, value); } }
        public ObservableCollection<FileInfo> DisplayFileInfoList { get { return m_DisplayFileInfoList; } set { SetProperty(ref m_DisplayFileInfoList, value); } }
        public FileInfo SelectedFileInfo { get { return m_SelectedFileInfo; } set { SetProperty(ref m_SelectedFileInfo, value); } }
        public ObservableCollection<string> ExtensionList { get { return m_ExtensionList; } set { SetProperty(ref m_ExtensionList, value); } }
        public bool IsFiltering { get { return m_IsFiltering; } set { SetProperty(ref m_IsFiltering, value); } }

        public DelegateCommand ProcessStartCommand { get; protected set; }
        public DelegateCommand ProcessCloseCommand { get; protected set; }
        public DelegateCommand RemoveItemCommand { get; protected set; }
        public DelegateCommand ChangeItemOrderUpperCommand { get; protected set; }
        public DelegateCommand ChangeItemOrderLowerCommand { get; protected set; }


        public FileAccessViewModel()
        {
            FileInfoList = new ObservableCollection<FileInfo>();
            DisplayFileInfoList = new ObservableCollection<FileInfo>();
            SelectedFileInfo = new FileInfo();
            ExtensionList = new ObservableCollection<string>();
            ExtensionList.Add("(指定なし)");
            ProcessCloseCommand = new DelegateCommand(ProcessClose, CanProcessClose);
            RemoveItemCommand = new DelegateCommand(RemoveItem, CanRemoveItem);
            ChangeItemOrderUpperCommand = new DelegateCommand(ChangeItemOrderUpper, CanChangeItemOrderUpper);
            ChangeItemOrderLowerCommand = new DelegateCommand(ChangeItemOrderLower, CanChangeItemOrderLower);
            ProcessStartCommand = new DelegateCommand(ProcessStart, CanProcessStart);
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
            bool result = SelectedFileInfo.Process.Start();
            SelectedFileInfo.Processing = true;
            SelectedFileInfo.Process.EnableRaisingEvents = true;
            SelectedFileInfo.Process.Exited += Process_Exited;

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
            FileInfoList.Remove(SelectedFileInfo);
            DisplayFileInfoList.Remove(SelectedFileInfo);

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
            FileInfoList.Move(index, index - 1);
            SynchronizeDisplayFileList();
        }

        protected bool CanChangeItemOrderLower()
        {
            return FileInfoList.Any() && SelectedFileInfo != null && FileInfoList.Count > FileInfoList.IndexOf(SelectedFileInfo) + 1 && !IsFiltering;
        }

        protected void ChangeItemOrderLower()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            FileInfoList.Move(index, index + 1);
            SynchronizeDisplayFileList();
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

        public void SynchronizeDisplayFileList()
        {
            DisplayFileInfoList.Clear();
            foreach (var item in FileInfoList)
            {
                DisplayFileInfoList.Add(item);
            }
        }
    }

    public class FileInfo : BindableBase
    {
        protected string m_FilePath;
        protected string m_FileName;
        protected BitmapSource m_Icon;
        protected Process m_Process = null;
        protected bool m_Processing = false;
        protected Process m_SettingProcess = null;

        public string FilePath { get { return m_FilePath; } set { SetProperty(ref m_FilePath, value); } }
        public string FileName { get { return m_FileName; } set { SetProperty(ref m_FileName, value); } }
        public BitmapSource Icon { get { return m_Icon; } set { SetProperty(ref m_Icon, value); } }
        public Process Process { get { return m_Process; } set { SetProperty(ref m_Process, value); } }
        public bool Processing { get { return m_Processing; } set { SetProperty(ref m_Processing, value); } }
        public Process SettingProcess { get { return m_SettingProcess; } set { SetProperty(ref m_SettingProcess, value); } }

    }
}
