namespace WPFAppFrameWork
{
	/// <summary>
	/// 選択可能な要素のインターフェース
	/// </summary>
	public interface ISelectableItem
	{
		#region プロパティ

		/// <summary>
		/// 選択されているか
		/// </summary>
		bool IsSelected { get; set; }

		#endregion
	}
}
