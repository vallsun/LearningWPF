using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFAppFrameWork.SoundPlayer.ViewModel;

namespace WPFAppFrameWork.SoundPlayer.View
{
	/// <summary>
	/// SounPlayerControl.xaml の相互作用ロジック
	/// </summary>
	public partial class SounPlayerControl : UserControl
	{
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
				};

				vm.StopRequested += (sender, e) =>
				{
					m_MediaElement.Stop();
				};

				vm.PauseRequested += (sender, e) =>
				{
					m_MediaElement.Pause();
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
			m_TimeLineSlider.Maximum = m_MediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
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

			// ループ再生が必要な場合には再度再生する
			var vm = m_MediaElement.DataContext as SoundPlayerViewModel;
			if (vm != null)
			{
				if (vm.NeedsLoop)
				{
					m_MediaElement.Play();
				}
			}
		}

		/// <summary>
		/// タイマーイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DispatcherTimer_Tick(object sender, EventArgs e)
		{
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
