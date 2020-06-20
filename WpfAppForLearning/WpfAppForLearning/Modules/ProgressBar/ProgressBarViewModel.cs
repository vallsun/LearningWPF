using System.ComponentModel;
using WpfAppForLearning.ViewModel;

namespace WpfAppForLearning.Modules.ProgressBar
{
	internal class ProgressBarViewModel : ContentViewModel
    {
        #region 構築・消滅

        public ProgressBarViewModel(INotifyPropertyChanged model)
            :base(model)
        {

        }

        #endregion
    }
}
