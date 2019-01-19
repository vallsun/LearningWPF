using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppForLearning.Modules.Common;

namespace WpfAppForLearning.Modules.KeyboardNavigation
{
    /// <summary>
    /// KeyboardNavigationコンテンツのVM
    /// </summary>
    public class KeyboardNavigationViewModel : BindableBase
    {
        private string n_NavigationMode1 = "Coontained";
        private string n_NavigationMode2 = "Coontained";

        public string NavigationMode1 { get { return n_NavigationMode1; } set { SetProperty(ref n_NavigationMode1, value); } }
        public string NavigationMode2 { get { return n_NavigationMode2; } set { SetProperty(ref n_NavigationMode2, value); } }

    }
}
