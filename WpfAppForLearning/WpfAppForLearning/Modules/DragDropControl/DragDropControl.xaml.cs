using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppForLearning.Modules.DragDropControl
{
    /// <summary>
    /// DragDropControl.xaml の相互作用ロジック
    /// </summary>
    public partial class DragDropControl : UserControl
    {
        public DragDropControl()
        {
            InitializeComponent();
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            var vm = this.DataContext as DragDropControlViewModel;
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
            }

                if (DuplicateFilePathList.Any())
                {
                    var message = "下記のアイテムは既に登録済みのため登録されませんでした。\n";
                    foreach(var item in DuplicateFilePathList)
                    {
                        message +=" " + item + "\n";
                    }
                    MessageBox.Show(message);
                }
            this.WatermarkTextBox.Visibility = Visibility.Collapsed;
        }

        private void UserControl_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
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
