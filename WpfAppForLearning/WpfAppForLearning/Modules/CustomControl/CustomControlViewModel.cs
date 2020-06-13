using System.ComponentModel;
using DevelopmentCommon.Common;

namespace WpfAppForLearning.Modules.CustomControl
{
	public class CustomControlViewModel : ViewModelBase
    {
        #region 構築・消滅

        public CustomControlViewModel(INotifyPropertyChanged model)
            : base(model)
        {

        }

        #endregion
    }
}
