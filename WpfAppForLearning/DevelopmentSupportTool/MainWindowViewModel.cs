using DevelopmentSupport.Common;
using DevelopmentSupport.Setting;
using System.Windows;

namespace DevelopmentSupportTool
{
    public class MainWindowViewModel :BindableBase
    {
        #region フィールド
        #endregion
        #region プロパティ

        public DelegateCommand ShowSettingCommand { get; private set; }
        public DelegateCommand ShowVersionInfoCommand { get; private set; }

        #endregion

        #region 構築・消滅

        public MainWindowViewModel()
        {
            ShowSettingCommand = new DelegateCommand(ShowSetting, CanShowSetting);
            ShowVersionInfoCommand = new DelegateCommand(ShowVersion, CanShowVersion);
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

        private bool CanShowVersion()
        {
            return true;
        }

        private void ShowVersion()
        {
            //自分自身のバージョン情報を取得する
            System.Diagnostics.FileVersionInfo ver =
                System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            MessageBox.Show(ver.FileVersion.ToString());
        }

        #endregion


    }
}
