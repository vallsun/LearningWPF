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

namespace DSCustomControlLibrary
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DSCustomControlLibrary"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DSCustomControlLibrary;assembly=DSCustomControlLibrary"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class BindableSelectedItemTreeView : TreeView
    {
        //
        // Bindable Definitions
        // - - - - - - - - - - - - - - - - - - - -

        public static readonly DependencyProperty BindableSelectedItemProperty
        #region...
        = DependencyProperty.Register(nameof(BindableSelectedItem),
                    typeof(object), typeof(BindableSelectedItemTreeView), new UIPropertyMetadata(null));
        #endregion

        //
        // Properties
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// Bind 可能な SelectedItem を表し、SelectedItem を設定または取得します。
        /// </summary>
        public object BindableSelectedItem
        {
            get { return (object)this.GetValue(BindableSelectedItemProperty); }
            set { this.SetValue(BindableSelectedItemProperty, value); }
        }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public BindableSelectedItemTreeView()
        {
            this.SelectedItemChanged += this.OnSelectedItemChanged;
        }

        //
        // Event Handlers
        // - - - - - - - - - - - - - - - - - - - -

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            this.SetValue(BindableSelectedItemProperty, this.SelectedItem);
        }
    }
}
