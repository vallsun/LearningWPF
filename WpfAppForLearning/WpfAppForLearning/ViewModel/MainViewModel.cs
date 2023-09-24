using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WpfAppForLearning.Modules.Common;
using WpfAppForLearning.Modules.ContentsTree.Model;
using WpfAppForLearning.Modules.CustomControl;
using WpfAppForLearning.Modules.DragDropControl;
using WpfAppForLearning.Modules.KeyboardNavigation;
using WpfAppForLearning.Modules.PathBarControl.ViewModel;
using WpfAppForLearning.Modules.ProgressBar;
using WpfAppForLearning.Modules.StartControl;
using WpfAppForLearning.Modules.TextBoxControl;
using WPFAppFrameWork;
using WPFAppFrameWork.XamlPad;

namespace WpfAppForLearning.ViewModel
{
	public class MainViewModel : SelectableViewModelBase<Content>
	{
		#region 定数定義

		private const string c_Content_Root = "コンテンツ";
		private const string c_ContentName_CustomControl = "CustomControl";

		#endregion

		#region フィールド

		/// <summary>
		/// 表示対象コンテンツのビューモデル
		/// </summary>
		private ContentViewModel m_ContentViewModel;

		/// <summary>
		/// ビューモデルのディクショナリ
		/// </summary>
		private readonly Dictionary<Content, ContentViewModel> m_ViewModelDictionary;

		#endregion

		#region プロパティ

		/// <summary>
		/// コンテンツ
		/// </summary>
		public Contents ContentsTree { get; set; }
		/// <summary>
		/// パスリストのビューモデル
		/// </summary>
		public PathBarViewModelImpl PathBar { get; set; }

		/// <summary>
		/// 表示対象コンテンツのビューモデル
		/// </summary>
		public ContentViewModel ContentViewModel
		{
			get
			{
				return m_ContentViewModel;
			}
			set
			{
				SetProperty(ref m_ContentViewModel, value);
			}
		}

		public DelegateCommand StartXamlPadCommand { get; set; }

		#endregion

		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainViewModel()
			: base(null)
		{
			//コンテンツの生成
			ContentsTree = new Contents();
			PathBar = new PathBarViewModelImpl(this, SelectedItem);
			m_ViewModelDictionary = new Dictionary<Content, ContentViewModel>();

			SelectedItem = ContentsTree.ContentsTree.FirstOrDefault();
		}

		#endregion

		#region 初期化

		protected override void RegisterCommands()
		{
			base.RegisterCommands();
			StartXamlPadCommand = new DelegateCommand(StartXamlPad, CanStartXamlPad);
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// 選択中のアイテムが変更された時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnSelectedItemChanged(sender, e);

			if (e.PropertyName != "SelectedItem")
			{
				return;
			}

			PathBar.OnSelectedItemChanged(sender, e);

			if (SelectedItem == null)
			{
				return;
			}

			//選択されたコンテンツに対応するVMを設定する
			if (!m_ViewModelDictionary.ContainsKey(SelectedItem))
			{
				ContentViewModel addVM = SelectedItem.Name switch
				{
					c_Content_Root => new StartControlViewModel(SelectedItem),
					c_ContentName_CustomControl => new CustomControlViewModel(SelectedItem),
					"ProgressBar" => new ProgressBarViewModel(SelectedItem),
					"KeyboardNavigation" => new KeyboardNavigationViewModel(),
					"DragDropControl" => new DragDropControlViewModel(),
					"TextBox" => new TextBoxControlViewModel(SelectedItem),
					_ => new NotImplementationViewModel(SelectedItem),
				};
				m_ViewModelDictionary.Add(SelectedItem, addVM);
			}

			ContentViewModel = m_ViewModelDictionary[SelectedItem];
		}

		#endregion

		#region 内部処理

		private void StartXamlPad()
		{
			App.WindowService.Show(new XamlPadWindowViewModel());
		}

		private bool CanStartXamlPad()
		{
			return true;
		}

		#endregion
	}
}
