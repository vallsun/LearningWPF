using System.ComponentModel;
using WpfAppForLearning.ViewModel;

namespace WpfAppForLearning.Modules.Common
{
	internal class NotImplementationViewModel : ContentViewModel
    {
        #region 構築・消滅

        public NotImplementationViewModel(INotifyPropertyChanged model)
            : base(model)
        {
            Description = "未実装";
        }

        #endregion
    }
}
