using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Windows.Shapes;
using WPFAppFrameWork.Common;
using WPFAppFrameWork.Service;
using Path = System.IO.Path;

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
		private Uri m_SoundSource = null;

		/// <summary>
		/// 音声ファイルのパス
		/// </summary>
		private string m_SoundSourceFileName = string.Empty;

		/// <summary>
		/// 視聴ファイルパスのリスト
		/// </summary>
		private ObservableCollection<Uri> m_SourceList = new ObservableCollection<Uri>();

		/// <summary>
		/// 選択中ファイルのパス
		/// </summary>
		private Uri m_SelectedSource = null;

		#endregion

		#region プロパティ

		/// <summary>
		/// 音声ファイルのパス
		/// </summary>
		public Uri SoundSource
		{
			get { return m_SoundSource; }
			set
			{
				if (SetProperty(ref m_SoundSource, value))
				{
					SoundSourceFileName = Path.GetFileName(m_SoundSource?.LocalPath);
				}
			}
		}

		/// <summary>
		/// 音声ファイルのパス
		/// </summary>
		public string SoundSourceFileName
		{
			get { return m_SoundSourceFileName; }
			set { SetProperty(ref m_SoundSourceFileName, value); }
		}

		/// <summary>
		/// 視聴ファイルパスのリスト
		/// </summary>
		public ObservableCollection<Uri> SourceList
		{
			get { return m_SourceList; }
			set { SetProperty(ref m_SourceList, value); }
		}

		/// <summary>
		/// 視聴ファイルパスのリストで選択中のファイル
		/// </summary>
		public Uri SelectedSource
		{
			get { return m_SelectedSource; }
			set
			{
				if (SetProperty(ref m_SelectedSource, value))
				{
					SoundSource = m_SelectedSource;
				}
			}
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

		/// <summary>
		/// ファイルを開く
		/// </summary>
		public DelegateCommand FileOpenCommand { get; protected set; }


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
			FileOpenCommand = new DelegateCommand(FileOpen, CanFileOpen);
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

		#region ファイルを開く

		private bool CanFileOpen()
		{
			return true;
		}

		private void FileOpen()
		{
			var filePath = FileService.OpenFileDialog();
			if(string.IsNullOrEmpty(filePath))
			{
				SoundSource = null;
				return;
			}
			SoundSource = new Uri(filePath);
			SourceList.Add(SoundSource);
		}
		#endregion

		#endregion
	}
}
