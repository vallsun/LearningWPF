using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using DevelopmentSupport.FileAccessor;

namespace DevelopmentSupport.FileStarter
{
    /// <summary>
    /// FileStarterView.xaml の相互作用ロジック
    /// </summary>
    public partial class FileStarterView : UserControl
    {
        public FileStarterView()
        {
            InitializeComponent();
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            var vm = this.DataContext as FileStarterViewModel;
            var list = vm.FileInfoList;
            var pathList = vm.FileInfoList.Select(x => x.FilePath);
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            var DuplicateFilePathList = new List<string>();

            if (files == null)
            {
                return;
            }
            foreach (var s in files)
            {
                if (Directory.Exists(s))
                {
                    continue;
                }
                if (pathList.Contains(s))
                {
                    DuplicateFilePathList.Add(s);
                    continue;
                }
                var fileInfo = new FileAccessor.FileInfo
                {
                    FilePath = s,
                    FileName = System.IO.Path.GetFileName(s)
                };
                if (File.Exists(s))
                {
                    var icon = System.Drawing.Icon.ExtractAssociatedIcon(s);
                    fileInfo.Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                list.Add(fileInfo);
                vm.SychronizeDisplayFileList();

                var extension = Path.GetExtension(s);
                if (!vm.ExtensionList.Contains(extension))
                {
                    vm.ExtensionList.Add(extension);
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
            this.WatermarkTextBox.Visibility = Visibility.Collapsed;
        }

        private void UserControl_PreviewDragOver(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            var fileFlag = false;
            foreach (var item in files)
            {
                if (File.Exists(item))
                {
                    fileFlag = true;
                    break;
                }
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true) && fileFlag)
                e.Effects = DragDropEffects.Copy;
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        /// <summary>
        /// コンボボックス項目が選択されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = this.DataContext as FileStarterViewModel;
            var filterkeyword = e.AddedItems[0].ToString();
            if (filterkeyword == "(指定なし)")
            {
                vm.SychronizeDisplayFileList();
                return;
            }
            var list = vm.FileInfoList;
            var filteredList = list.Where(x => Path.GetExtension(x.FilePath) == filterkeyword);
            if (!filteredList.Any())
            {
                MessageBox.Show("該当なし");
                return;
            }
            vm.DisplayFileInfoList.Clear();
            foreach (var item in filteredList)
            {
                vm.DisplayFileInfoList.Add(item);
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            var vm = this.DataContext as FileStarterViewModel;
            var list = vm.ExeInfoList;
            var pathList = vm.ExeInfoList.Select(x => x.FilePath);
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            var DuplicateFilePathList = new List<string>();

            if (files == null)
            {
                return;
            }
            foreach (var s in files)
            {
                if (Directory.Exists(s))
                {
                    continue;
                }
                if (pathList.Contains(s))
                {
                    DuplicateFilePathList.Add(s);
                    continue;
                }
                var fileInfo = new FileAccessor.FileInfo
                {
                    FilePath = s,
                    FileName = System.IO.Path.GetFileName(s)
                };
                if (File.Exists(s))
                {
                    var icon = System.Drawing.Icon.ExtractAssociatedIcon(s);
                    fileInfo.Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                list.Add(fileInfo);
                vm.SychronizeDisplayExeList();
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
            this.WatermarkTextBox2.Visibility = Visibility.Collapsed;
        }

        private void ListBox_PreviewDragOver(object sender, DragEventArgs e)
        {

        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };

    class Win32
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon  
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon  

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
    }
}
