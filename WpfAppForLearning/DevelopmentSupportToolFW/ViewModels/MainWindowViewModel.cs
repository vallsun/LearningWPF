using System.Windows;
using DevelopmentCommon.Common;
using DevelopmentSupport.FileAccessor.ViewModel;
using DevelopmentSupport.Setting;
using DevelopmentSupport.TaskList.ViewModel;

namespace DevelopmentSupportTool.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 現在のモード
        /// </summary>
        private ApplicationMode m_CurrentMode;

        private bool m_IsLauncherCurrent;

        private bool m_IsTaskListCurrent;

        private object m_CurrentPageViewModel;

        #endregion

        #region プロパティ

        /// <summary>
        /// 設定ビューを表示するコマンド
        /// </summary>
        public DelegateCommand ShowSettingCommand { get; private set; }

        /// <summary>
        /// バージョン情報を表示するコマンド
        /// </summary>
        public DelegateCommand ShowVersionInfoCommand { get; private set; }

        /// <summary>
        /// 現在のモード
        /// </summary>
        public ApplicationMode CurrentMode
        {
            get { return m_CurrentMode; }
            set
            {
	            var isSet = SetProperty(ref m_CurrentMode, value);
	            if (isSet)
	            {
		            OnCurrentModeChanged();
                }
            }
        }

        public FileAccessViewModel FileAccessViewModel { get; set; }

        public TaskListViewModel TaskListViewModel { get; set; }

        public object CurrentPageViewModel
        {
            get { return m_CurrentPageViewModel; }
            set { SetProperty(ref m_CurrentPageViewModel, value); }
        }

        public bool IsLauncherCurrent
        {
	        get { return m_IsLauncherCurrent; }
	        set { SetProperty(ref m_IsLauncherCurrent, value); }
        }

        public bool IsTakListCurrent
        {
	        get { return m_IsTaskListCurrent; }
	        set { SetProperty(ref m_IsTaskListCurrent, value); }
        }
        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        :base(null)
        {
            FileAccessViewModel = new FileAccessViewModel();
            TaskListViewModel = new TaskListViewModel();

	        // デフォルトはランチャモード
            CurrentMode = ApplicationMode.Luncher;
            CurrentPageViewModel = FileAccessViewModel;
            IsLauncherCurrent = true;
        }

        #endregion

        #region 初期化

        /// <summary>
        /// コマンド初期化
        /// </summary>
        protected override void RegisterCommands()
        {
            base.RegisterCommands();
            ShowSettingCommand = new DelegateCommand(ShowSetting, CanShowSetting);
            ShowVersionInfoCommand = new DelegateCommand(ShowVersion, CanShowVersion);

        }
        
        #endregion

        #region コマンド

        /// <summary>
        /// 設定ビューを表示可能か
        /// </summary>
        /// <returns></returns>
        private bool CanShowSetting()
        {
            return true;
        }

        /// <summary>
        /// 設定ビューを表示する
        /// </summary>
        private void ShowSetting()
        {
            var app = App.Current as App;
            app.ShowModalView(new SettingDialogViewModel());
        }

        /// <summary>
        /// バージョンダイアログを表示可能か
        /// </summary>
        /// <returns></returns>
        private bool CanShowVersion()
        {
            return true;
        }

        /// <summary>
        /// バージョンダイアログを表示する
        /// </summary>
        private void ShowVersion()
        {
            //自分自身のバージョン情報を取得する
            System.Diagnostics.FileVersionInfo ver =
                System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            MessageBox.Show(ver.FileVersion.ToString());
        }

        #endregion

        #region 公開サービス

        public void OnCurrentModeChanged()
        {
	        switch (CurrentMode)
	        {
                case ApplicationMode.Luncher:
	                IsLauncherCurrent = true;
	                IsTakListCurrent = false;
                    CurrentPageViewModel = FileAccessViewModel;
	                break;
                case ApplicationMode.TaskManager:
	                IsTakListCurrent = true;
	                IsLauncherCurrent = false;
                    CurrentPageViewModel = TaskListViewModel;
	                break;
                case ApplicationMode.ClipBoard:
	                IsTakListCurrent = false;
	                IsLauncherCurrent = false;
	                break;
                default:
	                break;
	        }
        }
        #endregion

    }

    /// <summary>
    /// アプリケーションモードの列挙
    /// </summary>
    public enum ApplicationMode
    {
        /// <summary>
        /// ランチャ
        /// </summary>
        Luncher,

        /// <summary>
        /// クリップボード（バグがあるため停止中）
        /// </summary>
        ClipBoard,

        /// <summary>
        /// タスク一覧
        /// </summary>
        TaskManager,
    }
}
