using System.Collections.Generic;

namespace WPFAppFrameWork
{
	/// <summary>
	/// 要素選択可能なクラスのインターフェース
	/// </summary>
	public interface ISelectable<T>
	{
		/// <summary>
		/// 選択中の要素
		/// </summary>
		T SelectedItem { get; set; }

		/// <summary>
		/// 選択中の要素群
		/// </summary>
		IEnumerable<T> SelectedItems { get; set; }
	}
}
