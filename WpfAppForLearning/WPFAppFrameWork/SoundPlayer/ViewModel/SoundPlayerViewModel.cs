using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WPFAppFrameWork.Common;

namespace WPFAppFrameWork.SoundPlayer.ViewModel
{
	/// <summary>
	/// 音楽プレーヤーのビューモデル
	/// </summary>
	public class SoundPlayerViewModel : ViewModelBase
	{
		#region 内部フィールド

		/// <summary>
		/// 音声ファイルのパス
		/// </summary>
		private Uri m_SoundSource;

		#endregion

		#region プロパティ

		/// <summary>
		/// 音声ファイルのパス
		/// </summary>
		public Uri SoundSource
		{
			get { return m_SoundSource; }
			set { SetProperty(ref m_SoundSource, value); }
		}

		/// <summary>
		/// 音声を再生する
		/// </summary>
		public DelegateCommand PlayCommand { get; protected set; }

		/// <summary>
		/// 音声を停止する
		/// </summary>
		public DelegateCommand StopCommand { get; protected set; }

		/// <summary>
		/// 音声を一時停止する
		/// </summary>
		public DelegateCommand PauseCommand { get; protected set; }


		#endregion


		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SoundPlayerViewModel()
			:base(null)
		{

		}

		#endregion

		#region 初期化

		/// <summary>
		/// コマンドの登録
		/// </summary>
		protected override void RegisterCommands()
		{
			base.RegisterCommands();
			PlayCommand = new DelegateCommand(Play, CanPlay);
			StopCommand = new DelegateCommand(Stop, CanStop);
			PauseCommand = new DelegateCommand(Pause, CanPause);
		}

		#endregion

		#region イベント

		public event EventHandler PlayRequested;

		public event EventHandler StopRequested;

		public event EventHandler PauseRequested; 

		#endregion

		#region 内部処理

		#region 再生

		/// <summary>
		/// 音声を再生できるか
		/// </summary>
		/// <returns></returns>
		private bool CanPlay()
		{
			return SoundSource != null;
		}

		/// <summary>
		/// 音声を再生する
		/// </summary>
		private void Play()
		{
			PlayRequested?.Invoke(this, null);
		}

		#endregion

		#region 停止

		/// <summary>
		/// 音声を停止できるか
		/// </summary>
		/// <returns></returns>
		private bool CanStop()
		{
			return SoundSource != null;
		}

		/// <summary>
		/// 音声を停止する
		/// </summary>
		private void Stop()
		{
			StopRequested?.Invoke(this, null);
		}

		#endregion

		#region 一時停止

		/// <summary>
		/// 音声を一時停止できるか
		/// </summary>
		/// <returns></returns>
		private bool CanPause()
		{
			return SoundSource != null;
		}

		/// <summary>
		/// 音声を一時停止する
		/// </summary>
		private void Pause()
		{
			PauseRequested?.Invoke(this, null);
		}

		#endregion

		#endregion
	}
}
