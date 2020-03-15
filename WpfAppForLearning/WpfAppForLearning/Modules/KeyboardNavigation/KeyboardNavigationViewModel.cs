﻿using DevelopmentSupport.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppForLearning.Modules.Common;

namespace WpfAppForLearning.Modules.KeyboardNavigation
{
    /// <summary>
    /// KeyboardNavigationコンテンツのVM
    /// </summary>
    internal class KeyboardNavigationViewModel : ViewModelBase
    {
        #region フィールド

        private string n_NavigationMode1 = "Coontained";
        private string n_NavigationMode2 = "Coontained";

        #endregion

        #region プロパティ

        public string NavigationMode1 { get { return n_NavigationMode1; } set { SetProperty(ref n_NavigationMode1, value); } }
        public string NavigationMode2 { get { return n_NavigationMode2; } set { SetProperty(ref n_NavigationMode2, value); } }

        #endregion

        #region 構築・消滅

        public KeyboardNavigationViewModel(INotifyPropertyChanged model)
            : base(model)
        {

        }

        #endregion
    }
}
