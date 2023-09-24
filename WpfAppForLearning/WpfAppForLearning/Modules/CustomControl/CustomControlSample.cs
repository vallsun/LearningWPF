using System.Windows;
using System.Windows.Controls;

namespace WpfAppForLearning.Modules.CustomControl
{
	/// <summary>
	/// テキストボックスに入力した文字をリスト要素として追加するコントロール
	/// </summary>
	public class CustomControlSample : Control
	{

		#region 構築・消滅

		/// <summary>
		/// メタデータ
		/// </summary>
		static CustomControlSample()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControlSample), new FrameworkPropertyMetadata(typeof(CustomControlSample)));
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CustomControlSample()
		{
		}

		#endregion

		#region

		// XAMLで定義されたボタン格納用変数
		private Button addButton;
		private Button removeButton;

		#endregion

		#region プロパティ

		/// <summary>
		/// 追加するテキスト
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(
				"Value",
				typeof(string),
				typeof(CustomControlSample),
				new PropertyMetadata("", ValueChanged));

		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// 追加するテキストプロパティが変更された時のイベントハンドラ
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

			var myTextBox = ((CustomControlSample)d).GetTemplateChild("myTextBox") as TextBox;
			if ((string)e.NewValue == "")
			{
				myTextBox.Text = "追加するテキストを入力";
			}
		}

		/// <summary>
		/// カスタムコントロールが生成された時のイベントハンドラ
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// 前のテンプレートのコントロールの後処理
			if (this.addButton != null)
			{
				this.addButton.Click -= this.AddClick;
			}
			if (this.removeButton != null)
			{
				this.removeButton.Click -= this.RemoveClick;
			}

			// テンプレートからコントロールの取得
			this.addButton = this.GetTemplateChild("AddButton") as Button;
			this.removeButton = this.GetTemplateChild("RemoveButton") as Button;

			// イベントハンドラの登録
			if (this.addButton != null)
			{
				this.addButton.Click += this.AddClick;
			}
			if (this.removeButton != null)
			{
				this.removeButton.Click += this.RemoveClick;
			}
		}

		/// <summary>
		/// 要素追加ボタンがクリックされた時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddClick(object sender, RoutedEventArgs e)
		{
			var listBox = ((CustomControlSample)(((Button)sender).TemplatedParent)).GetTemplateChild("CustomListBox") as ListBox;
			var textBox = ((CustomControlSample)(((Button)sender).TemplatedParent)).GetTemplateChild("myTextBox") as TextBox;
			if (textBox.Text != "")
			{
				listBox.Items.Add(textBox.Text);
			}
			textBox.Text = "";
		}

		/// <summary>
		/// 要素削除ボタンが生成された時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RemoveClick(object sender, RoutedEventArgs e)
		{
			var listBox = ((CustomControlSample)(((Button)sender).TemplatedParent)).GetTemplateChild("CustomListBox") as ListBox;
			var index = listBox.Items.IndexOf(listBox.SelectedItem);
			listBox.Items.Remove(listBox.SelectedItem);
			//要素削除後の選択アイテムの変更
			if (listBox.Items.Count == 0)
			{
				//何も選択しない
				return;
			}
			else if (index > listBox.Items.Count - 1)
			{
				index--;
			}
			listBox.SelectedItem = listBox.Items.GetItemAt(index);
		}

		#endregion

	}
}
