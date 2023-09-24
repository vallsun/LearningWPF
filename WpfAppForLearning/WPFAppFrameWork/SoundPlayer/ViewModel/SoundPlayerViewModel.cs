using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFAppFrameWork.Service;
using WPFAppFrameWork.SoundPlayer.Model;
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
		/// 再生状態
		/// </summary>
		private PlayState m_PlayState = PlayState.Stop;

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
		private ObservableCollection<MediaSource> m_SourceList = new ObservableCollection<MediaSource>();

		/// <summary>
		/// 選択中ファイルのパス
		/// </summary>
		private MediaSource m_SelectedSource = null;

		/// <summary>
		/// 音量
		/// </summary>
		private double m_Volume = 0.5;

		/// <summary>
		/// ミュートするか
		/// </summary>
		private bool m_IsMuted = false;

		/// <summary>
		/// 音量のキャッシュ
		/// </summary>
		private double m_VolumeCache = 0.5;

		/// <summary>
		/// ループ再生するか
		/// </summary>
		private bool m_NeedsLoop = false;

		/// <summary>
		/// ソースを再生可能か
		/// </summary>
		private bool m_CanPlaySource = false;

		#endregion

		#region プロパティ

		/// <summary>
		/// 再生状態
		/// </summary>
		public PlayState PlayState
		{
			get { return m_PlayState; }
			set { SetProperty(ref m_PlayState, value); }
		}

		/// <summary>
		/// 音声ファイルのパス
		/// </summary>
		public Uri SoundSource
		{
			get { return m_SoundSource; }
			set
			{
				Stop();
				if (SetProperty(ref m_SoundSource, value))
				{
					SoundSourceFileName = Path.GetFileName(m_SoundSource?.LocalPath);
				}
				Play();
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
		public ObservableCollection<MediaSource> SourceList
		{
			get { return m_SourceList; }
			set { SetProperty(ref m_SourceList, value); }
		}

		/// <summary>
		/// 視聴ファイルパスのリストで選択中のファイル
		/// </summary>
		public MediaSource SelectedSource
		{
			get { return m_SelectedSource; }
			set
			{
				if (SetProperty(ref m_SelectedSource, value))
				{
					SoundSource = m_SelectedSource?.Uri;
				}
			}
		}

		/// <summary>
		/// 音量
		/// </summary>
		public double Volume
		{
			get { return m_Volume; }
			set { SetProperty(ref m_Volume, value); }
		}

		/// <summary>
		/// ミュートするか
		/// </summary>
		public bool IsMuted
		{
			get { return m_IsMuted; }
			set
			{
				if (SetProperty(ref m_IsMuted, value))
				{
					if (m_IsMuted)
					{
						m_VolumeCache = Volume;
						Volume = 0.0;

					}
					else
					{
						Volume = m_VolumeCache;
					}
				}
			}
		}

		/// <summary>
		/// ループ再生するか
		/// </summary>
		public bool NeedsLoop
		{
			get { return m_NeedsLoop; }
			set { SetProperty(ref m_NeedsLoop, value); }
		}

		/// <summary>
		/// ソースを再生可能か
		/// </summary>
		public bool CanPlaySource
		{
			get { return m_CanPlaySource; }
			set
			{
				if (SetProperty(ref m_CanPlaySource, value) && value)
				{
					CommandManager.InvalidateRequerySuggested();
					if (CanPlay())
					{
						Play();
					}
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

		#region 再生履歴操作

		/// <summary>
		/// 要素を削除する
		/// </summary>
		public DelegateCommand<MediaSource> DeleteItemCommand { get; protected set; }

		/// <summary>
		/// 要素を上に移動
		/// </summary>
		public DelegateCommand<MediaSource> ChangeItemOrderUpperCommand { get; protected set; }

		/// <summary>
		/// 要素を下に移動
		/// </summary>
		public DelegateCommand<MediaSource> ChangeItemOrderLowerCommand { get; protected set; }

		#endregion

		#endregion

		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SoundPlayerViewModel()
			: base(null)
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
			DeleteItemCommand = new DelegateCommand<MediaSource>(DeleteItem, CanDeleteItem);
			ChangeItemOrderUpperCommand = new DelegateCommand<MediaSource>(ChangeItemOrderUpper, CanChangeItemOrderUpper);
			ChangeItemOrderLowerCommand = new DelegateCommand<MediaSource>(ChangeItemOrderLower, CanChangeItemOrderLower);
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
			return SoundSource != null && CanPlaySource && PlayState != PlayState.Play;
		}

		/// <summary>
		/// 音声を再生する
		/// </summary>
		private void Play()
		{
			if (SoundSource == null)
			{
				return;
			}
			PlayRequested?.Invoke(this, null);
			PlayState = PlayState.Play;
		}

		#endregion

		#region 停止

		/// <summary>
		/// 音声を停止できるか
		/// </summary>
		/// <returns></returns>
		private bool CanStop()
		{
			return SoundSource != null && CanPlaySource && PlayState != PlayState.Stop;
		}

		/// <summary>
		/// 音声を停止する
		/// </summary>
		private void Stop()
		{
			if (SoundSource == null)
			{
				return;
			}
			StopRequested?.Invoke(this, null);
			PlayState = PlayState.Stop;
		}

		#endregion

		#region 一時停止

		/// <summary>
		/// 音声を一時停止できるか
		/// </summary>
		/// <returns></returns>
		private bool CanPause()
		{
			return SoundSource != null && CanPlaySource && PlayState == PlayState.Play;
		}

		/// <summary>
		/// 音声を一時停止する
		/// </summary>
		private void Pause()
		{
			PauseRequested?.Invoke(this, null);
			PlayState = PlayState.Pause;
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
			if (string.IsNullOrEmpty(filePath))
			{
				SoundSource = null;
				return;
			}
			var mediaSource = new MediaSource(new Uri(filePath));
			SelectedSource = mediaSource;
			SourceList.Add(mediaSource);
			SoundSource = mediaSource.Uri;
		}
		#endregion

		#region 再生履歴操作

		#region 要素削除

		/// <summary>
		/// 要素を削除可能か
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private bool CanDeleteItem(MediaSource item)
		{
			return item != null;
		}

		/// <summary>
		/// 要素を削除する
		/// </summary>
		private void DeleteItem(MediaSource item)
		{
			if (!SourceList.Contains(item))
			{
				//　フェールセーフ
				return;
			}
			var index = SourceList.IndexOf(item);
			SourceList.Remove(item);
			if (SourceList.Count == 0)
			{
				SelectedSource = null;
				return;
			}
			if (index != 0)
			{
				SelectedSource = SourceList[index - 1];
			}
			else
			{
				SelectedSource = SourceList[index];
			}
		}

		#endregion

		#region 上へ移動

		/// <summary>
		/// 要素を上に移動可能か
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private bool CanChangeItemOrderUpper(MediaSource item)
		{
			return item != null && SourceList.IndexOf(item) != 0;
		}

		/// <summary>
		/// 要素を上に移動する
		/// </summary>
		/// <param name="item"></param>
		private void ChangeItemOrderUpper(MediaSource item)
		{
			if (!SourceList.Contains(item))
			{
				return;
			}
			var index = SourceList.IndexOf(item);
			SourceList.RemoveAt(index);
			SourceList.Insert(index - 1, item);
			SelectedSource = item;
		}

		#endregion

		#region 下へ移動

		/// <summary>
		/// 要素を下に移動可能か
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private bool CanChangeItemOrderLower(MediaSource item)
		{
			return item != null && SourceList.IndexOf(item) != SourceList.Count - 1;
		}

		/// <summary>
		/// 要素を下に移動する
		/// </summary>
		/// <param name="item"></param>
		private void ChangeItemOrderLower(MediaSource item)
		{
			if (!SourceList.Contains(item))
			{
				return;
			}
			var index = SourceList.IndexOf(item);
			SourceList.RemoveAt(index);
			SourceList.Insert(index + 1, item);
			SelectedSource = item;
		}

		#endregion

		#endregion

		#endregion
	}

	public enum PlayState
	{
		Stop,
		Play,
		Pause,
	}
}
