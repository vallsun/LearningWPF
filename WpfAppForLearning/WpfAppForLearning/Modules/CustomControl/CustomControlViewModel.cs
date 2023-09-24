using System.ComponentModel;
using WpfAppForLearning.ViewModel;

namespace WpfAppForLearning.Modules.CustomControl
{
	public class CustomControlViewModel : ContentViewModel
	{
		#region 構築・消滅

		public CustomControlViewModel(INotifyPropertyChanged model)
			: base(model)
		{

		}

		#endregion
	}
}
