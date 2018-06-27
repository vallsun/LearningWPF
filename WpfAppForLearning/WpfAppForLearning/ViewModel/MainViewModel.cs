using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppForLearning.Modules.ContentsTree.Model;

namespace WpfAppForLearning.ViewModel
{
    public class MainViewModel : ObservableCollection<ContentsTree>
    {
        public ObservableCollection<ContentsTree> Contents;

        public MainViewModel()
        {
            Contents = new ObservableCollection<ContentsTree>()
            {
                new ContentsTree(){
                    ContentName = "コンテンツ",
                    Children = new ObservableCollection<ContentsTree>()
                    {
                        new ContentsTree(){ContentName = "カスタムコントロール"},
                        new ContentsTree(){ContentName = "レイアウト"},
                    },
                },
            };
        }
    }
}
