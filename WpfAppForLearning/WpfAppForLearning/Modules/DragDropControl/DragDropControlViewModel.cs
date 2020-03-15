using DevelopmentSupport.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfAppForLearning.Modules.DragDropControl
{
    internal class DragDropControlViewModel : ViewModelBase
    {

        private ObservableCollection<FileInfo> m_FileInfoList;
        private FileInfo m_SelectedFileInfo;
        public ObservableCollection<FileInfo> FileInfoList { get { return m_FileInfoList; } set { SetProperty(ref m_FileInfoList, value); } }
        public FileInfo SelectedFileInfo { get { return m_SelectedFileInfo; } set { SetProperty(ref m_SelectedFileInfo, value); } }
        public DelegateCommand ProcessStartCommand { get; private set; }
        public DelegateCommand ProcessCloseCommand { get; private set; }
        public DelegateCommand RemoveItemCommand { get; private set; }
        public DelegateCommand ChangeItemOrderUpperCommand { get; private set; }
        public DelegateCommand ChangeItemOrderLowerCommand { get; private set; }


        public DragDropControlViewModel(INotifyPropertyChanged model)
            : base(model)
        {
            FileInfoList = new ObservableCollection<FileInfo>();
            SelectedFileInfo = new FileInfo();
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

        private bool CanProcessClose()
        {
            return SelectedFileInfo!= null && SelectedFileInfo.Process != null;
        }

        private void ProcessClose()
        {
            SelectedFileInfo.Process.CloseMainWindow();
            SelectedFileInfo.Process.Close();
            SelectedFileInfo.Process = null;
        }

        private bool CanRemoveItem()
        {
            return FileInfoList.Any() && SelectedFileInfo != null;
        }
    
        private void RemoveItem()
        {
            FileInfoList.Remove(SelectedFileInfo);
        }

        private bool CanChangeItemOrderUpper()
        {
            return FileInfoList.Any() && FileInfoList.IndexOf(SelectedFileInfo) > 0;
        }

        private void ChangeItemOrderUpper()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            FileInfoList.Move(index, index - 1);
        }
        private bool CanChangeItemOrderLower()
        {
            return FileInfoList.Any() && FileInfoList.Count > FileInfoList.IndexOf(SelectedFileInfo) + 1;
        }
        private void ChangeItemOrderLower()
        {
            var index = FileInfoList.IndexOf(SelectedFileInfo);
            FileInfoList.Move(index, index + 1);
        }
        private bool CanProcessStart()
        {
            if(SelectedFileInfo == null)
            {
                return false;
            }
            return FileInfoList.Any() && SelectedFileInfo.Process == null;
        }
        private void ProcessStart()
        {
            Execute();
        }

    }

    public class FileInfo : BindableBase
    {
        private string m_FilePath;
        private string m_FileName;
        private BitmapSource m_Icon;
        private Process m_Process = null;

        public string FilePath { get { return m_FilePath; } set { SetProperty(ref m_FilePath, value); } }
        public string FileName { get { return m_FileName; } set { SetProperty(ref m_FileName, value); } }
        public BitmapSource Icon { get { return m_Icon; } set { SetProperty(ref m_Icon, value); } }
        public Process Process { get { return m_Process; } set { SetProperty(ref m_Process, value); } }

    }
}
