using DevelopmentSupport.Common;
using DevelopmentSupport.FileAccessor;
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

namespace DevelopmentSupport.FileStarter
{
    public class FileStarterViewModel : FileAccessViewModel
    {
        protected ObservableCollection<FileAccessor.FileInfo> m_ExeInfoList;
        protected ObservableCollection<FileAccessor.FileInfo> m_DisplayExeInfoList;
        protected FileAccessor.FileInfo m_SelectedExeInfo;
        protected string m_TextEditorPath = "notepad";

        public ObservableCollection<FileAccessor.FileInfo> ExeInfoList { get { return m_ExeInfoList; } set { SetProperty(ref m_ExeInfoList, value); } }
        public ObservableCollection<FileAccessor.FileInfo> DisplayExeInfoList { get { return m_DisplayExeInfoList; } set { SetProperty(ref m_DisplayExeInfoList, value); } }
        public FileAccessor.FileInfo SelectedExeInfo { get { return m_SelectedExeInfo; } set { SetProperty(ref m_SelectedExeInfo, value); } }
        public string TextEditorPath { get { return m_TextEditorPath; } set { SetProperty(ref m_TextEditorPath, value); } }
        public DelegateCommand SettingProcessStartCommand { get; protected set; }

        public FileStarterViewModel()
            :base()
        {
            ExeInfoList = new ObservableCollection<FileAccessor.FileInfo>();
            DisplayExeInfoList = new ObservableCollection<FileAccessor.FileInfo>();
            SelectedExeInfo = new FileAccessor.FileInfo();

            SettingProcessStartCommand = new DelegateCommand(SettingProcessStart,CanSettingProcessStart);
        }

        public void SychronizeDisplayExeList()
        {
            DisplayExeInfoList.Clear();
            foreach (var item in ExeInfoList)
            {
                DisplayExeInfoList.Add(item);
            }
        }

        protected bool CanSettingProcessStart()
        {
            if (SelectedFileInfo == null)
            {
                return false;
            }
            return DisplayExeInfoList.Any() && SelectedExeInfo.SettingProcess == null;
        }
        protected void SettingProcessStart()
        {
            if (SelectedFileInfo == null)
            {
                return;
            }
            var configPath = SelectedExeInfo.FilePath + ".config";
            if (!File.Exists(configPath))
            {
                MessageBox.Show("設定ファイルが存在しません。\n指定されたパス:" + configPath);
                return;
            }

            if (!File.Exists(TextEditorPath) && TextEditorPath != "notepad")
            {
                MessageBox.Show("指定されたテキストエディタが見つかりません。\n指定されたパス：" + TextEditorPath);
                return;
            }

            StartProcessBySelectedApp(TextEditorPath, configPath);

        }
    }
}
