using System.Windows;
using System.Windows.Controls;

namespace WpfAppForLearning.Modules.ContentCustomControl
{
	/// <summary>
	/// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
	///
	/// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
	/// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
	/// 追加します:
	///
	///     xmlns:MyNamespace="clr-namespace:WpfAppForLearning.Modules.ContentCustomControl"
	///
	///
	/// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
	/// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
	/// 追加します:
	///
	///     xmlns:MyNamespace="clr-namespace:WpfAppForLearning.Modules.ContentCustomControl;assembly=WpfAppForLearning.Modules.ContentCustomControl"
	///
	/// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
	/// リビルドして、コンパイル エラーを防ぐ必要があります:
	///
	///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
	///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
	///
	///
	/// 手順 2)
	/// コントロールを XAML ファイルで使用します。
	///
	///     <MyNamespace:ContentCustomControl/>
	///
	/// </summary>
	public class ContentCustomControl : Control
	{
		/// <summary>
		/// メタデータ
		/// </summary>
		static ContentCustomControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentCustomControl), new FrameworkPropertyMetadata(typeof(ContentCustomControl)));
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ContentCustomControl()
		{

		}

		/// <summary>
		/// 要素削除ボタンが生成された時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RemoveClick(object sender, RoutedEventArgs e)
		{
			var listBox = ((ContentCustomControl)(((Button)sender).TemplatedParent)).GetTemplateChild("CustomListBox") as ListBox;
			listBox.Items.Remove(listBox.SelectedItem);
		}
	}
}
