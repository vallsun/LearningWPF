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
            var DuplicateFilePathList = new List<string>();


            if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files))
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
                var fileInfo = new FileInfo
                {
                    FilePath = s,
                    FileName = System.IO.Path.GetFileName(s)
                };
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
                if (!vm.ExtensionList.Select(x => x.Name).Contains(extension))
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
					vm.ExtensionList.Add(addExtension);
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

            WatermarkTextBox.Visibility = vm.FileInfoList.Any() ? Visibility.Collapsed : Visibility.Visible;
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
            if (vm == null)
            {
                return;
            }
            var filterExtension = ((Extension)e.AddedItems[0]).Name;
            vm.FilterByExtension(filterExtension);
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
