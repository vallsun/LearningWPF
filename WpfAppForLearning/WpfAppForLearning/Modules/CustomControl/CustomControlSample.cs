using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppForLearning.Modules.CustomControl
{
    /// <summary>
    /// テキストボックスに入力した文字をリスト要素として追加するコントロール
    /// </summary>
    public class CustomControlSample : Control
    {

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static CustomControlSample()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControlSample), new FrameworkPropertyMetadata(typeof(CustomControlSample)));
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
            if(Value != "")
            {
                listBox.Items.Add(Value);
            }
            Value = "";
        } 

        /// <summary>
        /// 要素削除ボタンが生成された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            var listBox = ((CustomControlSample)(((Button)sender).TemplatedParent)).GetTemplateChild("CustomListBox") as ListBox;
            listBox.Items.Remove(listBox.SelectedItem);
        }

        #endregion

    }
}
