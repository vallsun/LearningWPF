using System.ComponentModel;
using WPFAppFrameWork;

namespace WpfAppForLearning.ViewModel
{
	/// <summary>
	/// コンテンツのビューモデル
	/// </summary>
	public class ContentViewModel : ViewModelBase
	{
		#region フィールド

		/// <summary>
		/// 説明
		/// </summary>
		private string m_Description;

		/// <summary>
		/// 用途
		/// </summary>
		private string m_Usage;

		#endregion


		#region プロパティ

		/// <summary>
		/// 説明
		/// </summary>
		public string Description
		{
			get { return m_Description; }
			set { SetProperty(ref m_Description, value); }
		}

		/// <summary>
		/// 用途
		/// </summary>
		public string Usage
		{
			get => m_Usage;
			set => _ = SetProperty(ref m_Usage, value);
		}

		#endregion

		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="model"></param>
		public ContentViewModel(INotifyPropertyChanged model)
			: base(model)
		{

		}

		#endregion
	}
}
