using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using WPFAppFrameWork;

namespace DevelopmentSupport.Common.PathBar
{
	/// <summary>
	/// パスバーのVMベース
	/// </summary>
	public class PathBarViewModelBase<T> : ViewModelBase
		where T : HierarchicalItemBase<T>, INamable, ISelectableItem
	{
		#region プロパティ

		/// <summary>
		/// パスリストのビューモデルのコレクション
		/// </summary>
		public ObservableCollection<PathBarItemViewModelBase<T>> PathListViewModel { get; set; }

		/// <summary>
		/// パスバーを所持するビューモデル
		/// </summary>
		public SelectableViewModelBase<T> Owner { get; set; }

		public T SelectedItem
		{
			get { return Owner.SelectedItem; }
			set { Owner.SelectedItem = value; }
		}

		#endregion

		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="content"></param>
		public PathBarViewModelBase(SelectableViewModelBase<T> owner, T content)
			: base(null)
		{
			PropertyChanged += OnSelectedItemChanged;
			Owner = owner;
			PathListViewModel = new ObservableCollection<PathBarItemViewModelBase<T>>();
			if (content != null)
			{
				CreatePathList(content);
			}
		}

		#endregion

		#region 内部処理

		/// <summary>
		/// パスリストを生成
		/// </summary>
		/// <param name="content"></param>
		private void CreatePathList(T content)
		{
			Contract.Requires(PathListViewModel != null);
			if (content == null)
			{
				return;
			}

			//子要素から親要素に向かってリスト先頭に要素を追加
			PathListViewModel.Insert(0, new PathBarItemViewModelBase<T>(this, content));
			if (content.Parent != null)
			{
				CreatePathList(content.Parent);
			};
		}

		#endregion

		#region 公開サービス

		/// <summary>
		/// モデルに対応するVMを取得する
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public PathBarItemViewModelBase<T> GetItemVM(T model)
		{
			return PathListViewModel.FirstOrDefault(i => ((T)i.Model) == model);
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// 選択アイテムが変更された時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
		{

			if (Owner == null)
			{
				return;
			}
			PathListViewModel.Clear();
			//パスリストの再生成
			CreatePathList((T)Owner.SelectedItem);
		}

		#endregion
	}
}
