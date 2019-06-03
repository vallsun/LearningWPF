using DevelopmentSupport.Common;
using DevelopmentSupport.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupportTool
{
    public class MainWindowViewModel :BindableBase
    {
        #region フィールド
        #endregion
        #region プロパティ

        public DelegateCommand ShowSettingCommand { get; private set; }


        #endregion
        #region 構築・消滅

        public MainWindowViewModel()
        {
            ShowSettingCommand = new DelegateCommand(ShowSetting, CanShowSetting);
        }

        #endregion

        #region コマンド

        private bool CanShowSetting()
        {
            return true;
        }

        private void ShowSetting()
        {
            var app = App.Current as App;
            app.ShowModalView(new SettingDialogViewModel());
        }
        #endregion


    }
}
