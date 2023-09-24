using System.ComponentModel;
using WpfAppForLearning.ViewModel;

namespace WpfAppForLearning.Modules.StartControl
{
	public class StartControlViewModel : ContentViewModel
	{
		#region 構築・消滅

		public StartControlViewModel(INotifyPropertyChanged model)
			: base(model)
		{
			Description = "ようこそ！\r\nこのツールはWPFについて学ぶためのツールです。\r\n"
				+ "コンテンツナビゲータでコンテンツを選択してください。\r\n"
				+ "本ツールは開発中で、コンテンツや機能は今後追加予定です。";
		}

		#endregion
	}
}
