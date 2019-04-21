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
        public ObservableCollection<FileInfo> FileInfoList { get { return m_FileInfoList; } set { SetProperty(ref m_FileInfoList, value); } }
        public ObservableCollection<FileInfo> DisplayFileInfoList { get { return m_DisplayFileInfoList; } set { SetProperty(ref m_DisplayFileInfoList, value); } }
        public FileInfo SelectedFileInfo { get { return m_SelectedFileInfo; } set { SetProperty(ref m_SelectedFileInfo, value); } }
        public ObservableCollection<string> ExtensionList { get { return m_ExtensionList; } set { SetProperty(ref m_ExtensionList, value); } }
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
        }

        protected bool CanRemoveItem()
        {
            return FileInfoList.Any() && SelectedFileInfo != null;
        }

        protected void RemoveItem()
        {
            DisplayFileInfoList.Remove(SelectedFileInfo);
        }

        protected bool CanChangeItemOrderUpper()
        {
            return FileInfoList.Any() && SelectedFileInfo != null && FileInfoList.IndexOf(SelectedFileInfo) > 0;
        }

        protected void ChangeItemOrderUpper()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            DisplayFileInfoList.Move(index, index - 1);
        }

        protected bool CanChangeItemOrderLower()
        {
            return FileInfoList.Any() && SelectedFileInfo != null && FileInfoList.Count > FileInfoList.IndexOf(SelectedFileInfo) + 1;
        }

        protected void ChangeItemOrderLower()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            DisplayFileInfoList.Move(index, index + 1);
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

        public void SychronizeDisplayFileList()
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
        protected Process m_SettingProcess = null;

        public string FilePath { get { return m_FilePath; } set { SetProperty(ref m_FilePath, value); } }
        public string FileName { get { return m_FileName; } set { SetProperty(ref m_FileName, value); } }
        public BitmapSource Icon { get { return m_Icon; } set { SetProperty(ref m_Icon, value); } }
        public Process Process { get { return m_Process; } set { SetProperty(ref m_Process, value); } }
        public Process SettingProcess { get { return m_SettingProcess; } set { SetProperty(ref m_SettingProcess, value); } }

    }
}
