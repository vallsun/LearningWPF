using System;
using System.Diagnostics;

namespace DevelopmentCommon.Common
{
	/// <summary>
	/// プロセス実行サービス
	/// </summary>
	public static class ProcessService
	{
		#region 公開サービス

		/// <summary>
		/// 指定されたURLでプロセスを開始する
		/// </summary>
		/// <param name="url">URL</param>
		/// <remarks>例外処理は使用側で実施すること</remarks>
		public static void Navigate(Uri url)
		{
			ProcessStartInfo info = new ProcessStartInfo();
			info.FileName = url.AbsoluteUri;
			// .NET Coreから、パスの指定だけではURLを開く事ができないため、
			// ProcessStartInfo.UseShellExecuteをtrueに設定する必要がある
			// https://github.com/dotnet/runtime/issues/17938
			info.UseShellExecute = true;
			Process.Start(info);
		}

		#endregion
	}
}
