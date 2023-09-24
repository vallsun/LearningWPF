﻿using System.ComponentModel;
using WpfAppForLearning.ViewModel;

namespace WpfAppForLearning.Modules.TextBoxControl
{
	internal class TextBoxControlViewModel : ContentViewModel
	{
		#region 構築・消滅

		public TextBoxControlViewModel(INotifyPropertyChanged model)
			: base(model)
		{
			Usage = "書式なしテキストの表示または編集に使用できるコントロール";
		}

		#endregion
	}
}
