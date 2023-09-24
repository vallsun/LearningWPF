using System;
using System.IO;

namespace WPFAppFrameWork.SoundPlayer.Model
{
	/// <summary>
	/// メディアソース
	/// </summary>
	public class MediaSource
	{
		#region フィールド

		/// <summary>
		/// ソースのUri
		/// </summary>
		private Uri m_Uri;

		#endregion

		#region プロパティ

		/// <summary>
		/// ソースのUri
		/// </summary>
		public Uri Uri
		{
			get
			{
				return m_Uri;
			}
		}

		/// <summary>
		/// ソースのファイル名
		/// </summary>
		public string FileName
		{
			get
			{
				return Path.GetFileName(Uri?.LocalPath);
			}
		}

		#endregion

		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MediaSource(Uri uri)
		{
			m_Uri = uri;
		}

		#endregion
	}
}
