using DevelopmentSupport.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
