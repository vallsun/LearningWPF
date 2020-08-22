using System.Windows;
using Microsoft.Win32;

namespace WPFAppFrameWork.Service
{
	public static class FileService
	{
		public static string OpenFileDialog()
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == true)
			{
				return dialog.FileName;
			}

			return string.Empty;
		}
	}
}
