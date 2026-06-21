using Microsoft.Win32;

namespace WPFAppFrameWork.Service
{
	/// <summary>
	/// GUIによるファイルサービス群を提供するクラス。
	/// </summary>
	public static class FileService
	{
		/// <summary>
		/// ファイルオープン
		/// </summary>
		/// <returns></returns>
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
