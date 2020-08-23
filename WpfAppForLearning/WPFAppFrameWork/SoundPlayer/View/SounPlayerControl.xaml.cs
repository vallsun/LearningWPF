using System;
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


		public bool SoundOnly
		{
			get { return (bool)GetValue(SoundOnlyProperty); }
			set { SetValue(SoundOnlyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SoundOnly.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SoundOnlyProperty =
			DependencyProperty.Register("SoundOnly", typeof(bool), typeof(SounPlayerControl), new PropertyMetadata(false));

		/// <summary>
		/// スライダー更新用タイマー
		/// </summary>
		private DispatcherTimer m_timer;

		private bool IsUpdatingSlider = false;

		public SounPlayerControl()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var vm = m_MediaElement.DataContext as SoundPlayerViewModel;
			if (vm != null)
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
		private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
		{
			if(IsUpdatingSlider)
			{
				return;
			}
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
			if (!m_MediaElement.NaturalDuration.HasTimeSpan)
			{
				m_TimeLineSlider.Maximum = 0.0;
			}
			else
			{
				m_TimeLineSlider.Maximum = m_MediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
			}
			
			IntializeTimer();
		}

		private void IntializeTimer()
		{
			// タイマー設定
			m_timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(100)
			};
			m_timer.Tick += new EventHandler(DispatcherTimer_Tick);
			m_timer.Start();
		}

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
			var vm = m_MediaElement.DataContext as SoundPlayerViewModel;
			if (vm == null)
			{
				return;
			}

			var oldCanPlaySource = vm.CanPlaySource;
			var newCanPlaySource = m_MediaElement.HasAudio || m_MediaElement.HasVideo;
			SoundOnly = m_MediaElement.HasAudio && !m_MediaElement.HasVideo;
			if (oldCanPlaySource != newCanPlaySource)
			{
				vm.CanPlaySource = newCanPlaySource;
				CommandManager.InvalidateRequerySuggested();
			}

			if(vm.PlayState != PlayState.Play)
			{
				return;
			}
			IsUpdatingSlider = true;
			// 動画経過時間に合わせてスライダーを動かす
			double dbPrg = GetMovieProgress();
			m_TimeLineSlider.Value = dbPrg * m_TimeLineSlider.Maximum;

			IsUpdatingSlider = false;

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
