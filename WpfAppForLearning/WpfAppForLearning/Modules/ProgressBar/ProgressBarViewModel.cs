using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAppForLearning.ViewModel;
using WPFAppFrameWork;

namespace WpfAppForLearning.Modules.ProgressBar
{
	internal class ProgressBarViewModel : ContentViewModel
	{
		#region 内部フィールド

		/// <summary>
		/// プログレスバーの最大値
		/// </summary>
		double m_Maximum = 1.0;

		/// <summary>
		/// プログレスバーの最小値
		/// </summary>
		double m_Minimum = 0.0;

		/// <summary>
		/// プログレスバーの現在の進捗値
		/// </summary>
		double m_CurrentValue = 0.0;

		/// <summary>
		/// タスクキャンセル用のトークン
		/// </summary>
		private CancellationTokenSource cancellationTokenSource;

		#endregion


		#region プロパティ

		/// <summary>
		/// プログレスバーの最大値
		/// </summary>
		public double Maximum
		{
			get
			{
				return m_Maximum;
			}
			set
			{
				SetProperty(ref m_Maximum, value);
			}
		}

		/// <summary>
		/// プログレスバーの最小値
		/// </summary>
		public double Minimum
		{
			get
			{
				return m_Minimum;
			}
			set
			{
				SetProperty(ref m_Minimum, value);
			}
		}

		/// <summary>
		/// プログレスバーの現在の進捗値
		/// </summary>
		public double CurrentValue
		{
			get
			{
				return m_CurrentValue;
			}
			set
			{
				SetProperty(ref m_CurrentValue, value);
			}
		}

		/// <summary>
		/// タスク開始コマンド
		/// </summary>
		public ICommand StartTaskCommand { get; set; }

		/// <summary>
		/// タスクキャンセルコマンド
		/// </summary>
		public ICommand CancelTaskCommand { get; set; }

		#endregion

		#region 構築・消滅

		public ProgressBarViewModel(INotifyPropertyChanged model)
			: base(model)
		{

		}

		#endregion

		#region 初期化

		protected override void RegisterCommands()
		{
			base.RegisterCommands();
			StartTaskCommand = new DelegateCommand(StartTask, CanStartTask);
			CancelTaskCommand = new DelegateCommand(CancelTask, CanCancelTask);
		}

		#endregion

		#region 内部処理

		/// <summary>
		/// タスクを開始可能か
		/// </summary>
		/// <returns></returns>
		private bool CanStartTask()
		{
			return true;
		}

		/// <summary>
		/// タスクを開始する
		/// </summary>
		private async void StartTask()
		{
			CurrentValue = 0.0;
			var p = new Progress<double>(UpdateCurrentValue);
			cancellationTokenSource = new CancellationTokenSource();

			// 時間のかかる処理を別スレッドで開始
			await Task.Run(() => DoWork(p, 10), cancellationTokenSource.Token);

		}

		private bool CanCancelTask()
		{
			return true;
		}

		// 処理をキャンセルする
		private void CancelTask()
		{
			cancellationTokenSource.Cancel();
		}

		/// <summary>
		/// 現在の進捗値を更新する
		/// </summary>
		/// <param name="percent">現在の進捗値</param>
		private void UpdateCurrentValue(double percent)
		{
			CurrentValue = percent;
		}

		/// <summary>
		/// 時間の書かかる処理を実行する
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		private void DoWork(IProgress<double> progress, int n)
		{
			// 別スレッドで実行されるため、このメソッドでは
			// UI（コントロール）を操作してはいけない
			try
			{
				// 時間のかかる処理
				for (int i = 1; i <= n; i++)
				{
					// キャンセル処理
					cancellationTokenSource.Token.ThrowIfCancellationRequested();

					Thread.Sleep(100);

					int percentage = i * 100 / n; // 進捗率
					progress.Report(percentage);
				}
			}
			catch (OperationCanceledException)
			{
				// キャンセルされた場合
				progress.Report(0.0);
				return;
			}
		}

		#endregion
	}
}
