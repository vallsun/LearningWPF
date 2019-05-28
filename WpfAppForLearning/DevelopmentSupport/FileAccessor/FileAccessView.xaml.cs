using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace DevelopmentSupport.FileAccessor
{
    /// <summary>
    /// FileAccessView.xaml の相互作用ロジック
    /// </summary>
    public partial class FileAccessView : UserControl
    {
        public FileAccessView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ファイルをドラッグした時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            var vm = this.DataContext as FileAccessViewModel;
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
                if (pathList.Contains(s))
                {
                    DuplicateFilePathList.Add(s);
                    continue;
                }
                var fileInfo = new FileInfo();
                fileInfo.FilePath = s;
                fileInfo.FileName = System.IO.Path.GetFileName(s);
                if (File.Exists(s))
                {
                    var icon = System.Drawing.Icon.ExtractAssociatedIcon(s);
                    fileInfo.Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                else if (Directory.Exists(s))
                {
                    var shinfo = new SHFILEINFO();
                    var hImgSmall = Win32.SHGetFileInfo(s, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
                    var icon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
                    fileInfo.Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                }
                list.Add(fileInfo);
                vm.SynchronizeDisplayFileList();

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
            if (vm.FileInfoList.Any())
            {
                WatermarkTextBox.Visibility = Visibility.Collapsed;
            }   
        }

        private void UserControl_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        /// <summary>
        /// コンボボックス項目が選択されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = this.DataContext as FileAccessViewModel;
            if (!vm.DisplayFileInfoList.Any())
            {
                return;
            }
            var filterkeyword = e.AddedItems[0].ToString();
            if (filterkeyword == "(指定なし)")
            {
                vm.SynchronizeDisplayFileList();
                vm.IsFiltering = false;
                return;
            }
            var list = vm.FileInfoList;
            var filteredList = list.Where(x => Path.GetExtension(x.FilePath) == filterkeyword);
            if(!filteredList.Any())
            {
                MessageBox.Show("該当なし");
                return;
            }
            vm.DisplayFileInfoList.Clear();
            foreach (var item in filteredList)
            {
                vm.DisplayFileInfoList.Add(item);
            }
            vm.IsFiltering = true;
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
