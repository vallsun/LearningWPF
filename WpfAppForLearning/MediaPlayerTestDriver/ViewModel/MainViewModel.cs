using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using WPFAppFrameWork.SoundPlayer.ViewModel;

namespace MediaPlayerTestDriver.ViewModel
{
	public class MainViewModel
	{
		#region プロパティ

		/// <summary>
		/// 音声プレイヤーのビューモデル
		/// </summary>
		public SoundPlayerViewModel SoundPlayerViewModel { get; set; }

		#endregion

		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainViewModel()
		{
			SoundPlayerViewModel = new SoundPlayerViewModel();
			SoundPlayerViewModel.SoundSource = new Uri(@"C:\aaa.mp3");
		}
		#endregion
	}
}
