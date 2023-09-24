using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WPFAppFrameWork.SoundPlayer.ViewModel;

namespace WPFAppFrameWork.SoundPlayer.View
{
	/// <summary>
	/// SounPlayerControl.xaml の相互作用ロジック
	/// </summary>
	public partial class SounPlayerControl : UserControl
	{
		#region 内部フィールド

		/// <summary>
		/// タイマ発火のインターバル
		/// </summary>
		private double m_TimerTickInterval = 100;

		/// <summary>
		/// スライダー更新用タイマー
		/// </summary>
		private DispatcherTimer m_timer;

		/// <summary>
		/// スライダを更新中か
		/// </summary>
		private bool IsUpdatingSlider = false;

		/// <summary>
		/// シーク中か
		/// </summary>
		private bool IsSeeking = false;

		#endregion

		public bool SoundOnly
		{
			get { return (bool)GetValue(SoundOnlyProperty); }
			set { SetValue(SoundOnlyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SoundOnly.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SoundOnlyProperty =
			DependencyProperty.Register("SoundOnly", typeof(bool), typeof(SounPlayerControl), new PropertyMetadata(false));



		public SounPlayerControl()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (m_MediaElement.DataContext is SoundPlayerViewModel vm)
			{
				vm.PlayRequested += (sender, e) =>
				{
					m_MediaElement.Play();
					vm.PlayState = PlayState.Play;
				};

				vm.StopRequested += (sender, e) =>
				{
					m_MediaElement.Stop();
					vm.PlayState = PlayState.Stop;
					m_MediaElement.Close();
				};

				vm.PauseRequested += (sender, e) =>
				{
					m_MediaElement.Pause();
					vm.PlayState = PlayState.Pause;
				};
			}
		}

		/// <summary>
		/// スライダーの位置をもとにメディアの位置を変更する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <remarks>
		/// シーク操作によるスライダの位置変更の場合、完了タイミングを明示的に補足できないため、
		/// タイマイベント発火時の処理においてシーク完了時の処理実行が必要。<see cref="OnSeekEnded"/>
		/// </remarks>
		private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
		{
			// 再生中のスライダー更新処理によるイベント発火の場合には、何もしない
			if (IsUpdatingSlider)
			{
				return;
			}

			IsSeeking = true;

			// スライダ操作中は再生を一旦停止する
			var vm = m_MediaElement.DataContext as SoundPlayerViewModel;
			m_MediaElement.Pause();
			vm.PlayState = PlayState.Pause;

			int SliderValue = (int)m_TimeLineSlider.Value;

			// Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
			// Create a TimeSpan with miliseconds equal to the slider value.
			TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
			m_MediaElement.Position = ts;


		}

		/// <summary>
		/// メディアが開かれたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Element_MediaOpened(object sender, EventArgs e)
		{
			// スライダの初期化
			if (!m_MediaElement.NaturalDuration.HasTimeSpan)
			{
				m_TimeLineSlider.Maximum = 0.0;
			}
			else
			{
				m_TimeLineSlider.Maximum = m_MediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
			}
			
			// タイマの初期化
			IntializeTimer();
		}

		/// <summary>
		/// タイマの初期化
		/// </summary>
		private void IntializeTimer()
		{
			// タイマー設定
			m_timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(m_TimerTickInterval)
			};
			m_timer.Tick += new EventHandler(DispatcherTimer_Tick);
			m_timer.Start();
		}

		/// <summary>
		/// タイマのリセット
		/// </summary>
		private void ClearTimer()
		{
			if(m_timer == null)
			{
				return;
			}
			m_timer.Tick -= DispatcherTimer_Tick;
			m_timer = null;

		}

		/// <summary>
		/// メディアが終了したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Element_MediaEnded(object sender, EventArgs e)
		{
			// 停止する
			m_MediaElement.Stop();
			var vm = m_MediaElement.DataContext as SoundPlayerViewModel;
			if(vm != null)
			{
				vm.PlayState = PlayState.Stop;
			}
			m_MediaElement.Close();

			// ループ再生が必要な場合には再度再生する
			if (vm != null)
			{
				if (vm.NeedsLoop)
				{
					m_MediaElement.Play();
					vm.PlayState = PlayState.Play;
				}
			}

			CommandManager.InvalidateRequerySuggested();
		}

		/// <summary>
		/// タイマーイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DispatcherTimer_Tick(object sender, EventArgs e)
		{
			if (!(m_MediaElement.DataContext is SoundPlayerViewModel vm))
			{
				return;
			}

			var oldCanPlaySource = vm.CanPlaySource;
			var newCanPlaySource = m_MediaElement.HasAudio || m_MediaElement.HasVideo;
			SoundOnly = m_MediaElement.HasAudio && !m_MediaElement.HasVideo;
			if (oldCanPlaySource != newCanPlaySource)
			{
				// 再生可否が更新された場合には、強制的にコマンド実行可否を更新する。
				vm.CanPlaySource = newCanPlaySource;
				CommandManager.InvalidateRequerySuggested();
			}

			if(vm.PlayState != PlayState.Play)
			{
				// シーク操作の完了タイミングを明示的に補足できないため、タイマイベントでシーク完了とする
				if(IsSeeking)
				{
					OnSeekEnded(vm);
				}
				return;
			}

			// メディア再生中の場合には、動画経過時間に合わせてスライダーを動かす
			IsUpdatingSlider = true;
			double dbPrg = GetMovieProgress();
			m_TimeLineSlider.Value = dbPrg * m_TimeLineSlider.Maximum;
			IsUpdatingSlider = false;

		}

		/// <summary>
		/// シーク完了時の処理
		/// </summary>
		private void OnSeekEnded(SoundPlayerViewModel vm)
		{
			IsSeeking = false;
			// VMの再生状態プロパティを変更を起点としたイベント処理では再生状態の更新ができないため、
			// MediaElementのPlay()メソッドを即時実行する
			vm.PlayState = PlayState.Play;
			m_MediaElement.Play();
			// 短時間のシーク操作時、コマンド実行可否スライドバーの更新タイミングが影響して
			// コマンド実行可否が正しく更新されないため、強制的にコマンド実行可否を更新する。
			CommandManager.InvalidateRequerySuggested();
		}

		/// <summary>
		/// MediaElement 再生位置取得
		/// </summary>
		/// <returns></returns>
		private double GetMovieProgress()
		{
			if(!m_MediaElement.NaturalDuration.HasTimeSpan)
			{
				return 0.0;
			}
			TimeSpan tsCrnt = m_MediaElement.Position;
			TimeSpan tsDuration = m_MediaElement.NaturalDuration.TimeSpan;
			double dbPrg = tsCrnt.TotalMilliseconds / tsDuration.TotalMilliseconds;
			return dbPrg;
		}
	}
}
