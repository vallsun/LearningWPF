using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevelopmentSupport.Common.Selectable;
using WpfAppForLearning.Modules.Common;
using WpfAppForLearning.Modules.ContentsTree.Model;
using WpfAppForLearning.Modules.CustomControl;
using WpfAppForLearning.Modules.DragDropControl;
using WpfAppForLearning.Modules.KeyboardNavigation;
using WpfAppForLearning.Modules.PathBarControl.ViewModel;
using WpfAppForLearning.Modules.ProgressBar;
using WpfAppForLearning.Modules.StartControl;
using WpfAppForLearning.Modules.TextBoxControl;
using WPFAppFrameWork.Common;
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
		private ViewModelBase m_ContentViewModel;

		/// <summary>
		/// ビューモデルのディクショナリ
		/// </summary>
		private Dictionary<Content, ViewModelBase> m_ViewModelDictionary;

		#endregion

		#region プロパティ

		/// <summary>
		/// コンテンツ
		/// </summary>
		public Contents ContentsTree { get; set; }
		/// <summary>
		/// パスリストのビューモデル
		/// </summary>
		public PathBarViewModel PathBar { get; set; }

		/// <summary>
		/// 表示対象コンテンツのビューモデル
		/// </summary>
		public ViewModelBase ContentViewModel
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

		private static readonly PropertyChangedEventArgs SelectedItemPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedItem));

		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainViewModel()
			: base(null)
		{
			//コンテンツの生成
			ContentsTree = new Contents();
			PathBar = new PathBarViewModel(this, SelectedItem);
			m_ViewModelDictionary = new Dictionary<Content, ViewModelBase>();

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
				ViewModelBase addVM = null;
				switch (SelectedItem.Name)
				{
					case c_Content_Root:
						addVM = new StartControlViewModel(SelectedItem);
						break;
					case c_ContentName_CustomControl:
						addVM = new CustomControlViewModel(SelectedItem);
						break;
					case "ProgressBar":
						addVM = new ProgressBarViewModel(SelectedItem);
						break;
					case "KeyboardNavigation":
						addVM = new KeyboardNavigationViewModel();
						break;
					case "DragDropControl":
						addVM = new DragDropControlViewModel();
						break;
					case "TextBox":
						addVM = new TextBoxControlViewModel(SelectedItem);
						break;
					default:
						addVM = new NotImplementationViewModel(SelectedItem);
						break;
				}
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
