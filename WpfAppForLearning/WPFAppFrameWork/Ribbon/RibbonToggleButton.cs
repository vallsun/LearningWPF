﻿using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPFAppFrameWork.Ribbon
{
	/// <summary>
	/// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
	///
	/// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
	/// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
	/// 追加します:
	///
	///     xmlns:MyNamespace="clr-namespace:WPFAppFrameWork.Ribbon"
	///
	///
	/// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
	/// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
	/// 追加します:
	///
	///     xmlns:MyNamespace="clr-namespace:WPFAppFrameWork.Ribbon;assembly=WPFAppFrameWork.Ribbon"
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
	///     <MyNamespace:RibbonToggleButton/>
	///
	/// </summary>
	public class RibbonToggleButton : ToggleButton
    {
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(RibbonToggleButton), new PropertyMetadata(null));

        static RibbonToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonToggleButton), new FrameworkPropertyMetadata(typeof(RibbonToggleButton)));
        }
    }
}