using System.Collections.ObjectModel;
using DevelopmentSupport.Common.Hierarchical;
using DevelopmentSupport.Common.Namable;
using DevelopmentSupport.Common.Selectable;
using WpfAppForLearning.Properties;

namespace WpfAppForLearning.Modules.ContentsTree.Model
{
	/// <summary>
	/// コンテンツのツリーリスト
	/// </summary>
	public class Content : HierarchicalItemBase<Content>, INamable, ISelectableItem
	{
		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parent"></param>
		public Content()
		{
		}

		#endregion

		#region INamableメンバ

		/// <summary>
		/// 名前
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region ISelectableItemメンバ

		/// <summary>
		/// 選択中か
		/// </summary>
		public bool IsSelected { get; set; }

		#endregion

		#region 公開メソッド

		/// <summary>
		/// 子要素の追加
		/// </summary>
		/// <param name="child"></param>
		public void AddChild(Content child)
		{
			if (child == null)
			{
				return;
			}
			Children.Add(child);
			child.Parent = this;
		}

		#endregion

	}

	/// <summary>
	/// コンテンツ
	/// </summary>
	public class Contents
	{
		/// <summary>
		/// コンテンツ
		/// </summary>
		public ObservableCollection<Content> ContentsTree { get; set; }

		public Contents()
		{
			ContentsBuilder();
		}

		/// <summary>
		/// コンテンツの生成
		/// </summary>
		private void ContentsBuilder()
		{
			ContentsTree = new ObservableCollection<Content>();
			var rootContent = new Content()
			{
				Name = "コンテンツ",
				Children = new ObservableCollection<Content>(),
			};

			var controlCotent = new Content()
			{
				Name = Strings.ContentName_Control,
				Children = new ObservableCollection<Content>(),
			};

			var ProgressBarContent = new Content()
			{
				Name = Strings.ContentName_ProgressBar,
			};

			var userControlCotent = new Content()
			{
				Name = Strings.ContentName_UserControl,
			};

			var customControlCotent = new Content()
			{
				Name = Strings.ContentName_CustomControl,
			};

			var bindingCotent = new Content()
			{
				Name = "バインディング",
			};

			var converterCotent = new Content()
			{
				Name = "コンバータ",
			};

			var keyboardNavigationContent = new Content()
			{
				Name = "KeyboardNavigation"
			};

			var dragDropControlContent = new Content()
			{
				Name = Strings.ContentName_DroppableControl,
			};

			var layoutContent = new Content()
			{
				Name = "レイアウト",
				Children = new ObservableCollection<Content>(),
			};

			var panelContent = new Content()
			{
				Name = "パネル(Panel)",
				Children = new ObservableCollection<Content>(),
			};

			var stackPanelContent = new Content()
			{
				Name = "StackPanel",
			};

			var dockPanelContent = new Content()
			{
				Name = "DockPanel",
			};

			var wrapPanelContent = new Content()
			{
				Name = "WrapPanel",
			};

			var CanvasContent = new Content()
			{
				Name = "Canvas",
			};

			var TextBoxContent = new Content()
			{
				Name = "TextBox",
			};

			//レイアウト
			layoutContent.AddChild(panelContent);
			panelContent.AddChild(stackPanelContent);
			panelContent.AddChild(dockPanelContent);
			panelContent.AddChild(wrapPanelContent);
			panelContent.AddChild(CanvasContent);

			//コントロール
			controlCotent.AddChild(ProgressBarContent);
			controlCotent.AddChild(TextBoxContent);

			rootContent.AddChild(controlCotent);
			rootContent.AddChild(userControlCotent);
			rootContent.AddChild(customControlCotent);
			rootContent.AddChild(bindingCotent);
			rootContent.AddChild(keyboardNavigationContent);
			rootContent.AddChild(dragDropControlContent);
			rootContent.AddChild(layoutContent);
			ContentsTree.Add(rootContent);
		}
	}
}
