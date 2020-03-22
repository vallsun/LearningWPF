﻿using System;
using System.Collections.Generic;
using System.Windows;
using DevelopmentSupport.Setting;

namespace DevelopmentSupportTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Dictionary<Type, Type> ViewModels { get; set; }

        public App() : base()
        {
            // ViewModel と View の対応を設定する
            ViewModels = new Dictionary<Type, Type>();
            ViewModels.Add(typeof(SettingDialogViewModel), typeof(SettingDialog));
        }

        // ViewModelからViewを生成する
        public Window CreateView<T>(T viewModel)
        {
            // ViewModel に対応する Viewが存在する？
            if (ViewModels.ContainsKey(viewModel.GetType()))
            {
                // View を生成し、DataContext に ViewModel を設定する
                Type viewType = ViewModels[viewModel.GetType()];
                Window wnd = Activator.CreateInstance(viewType) as Window;
                if (wnd != null)
                    wnd.DataContext = viewModel;
                return wnd;
            }
            else
                return null;
        }

        // ViewModelからモーダルでViewを表示する
        public bool ShowModalView<T>(T viewModel)
        {
            Window view = CreateView(viewModel);
            if (view != null)
                return (view.ShowDialog() == true);
            else
                return false;
        }

        // ViewModeからモードレスでViewを表示する
        public void ShowView<T>(T viewModel)
        {
            Window view = CreateView(viewModel);
            if (view != null)
                view.Show();
        }
    }
}
