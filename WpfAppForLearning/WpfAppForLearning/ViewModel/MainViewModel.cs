using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppForLearning.Modules.ContentsTree.Model;

namespace WpfAppForLearning.ViewModel
{
    public class MainViewModel
    {
        public List<ContentsTree> Contents { get; set; }

        public MainViewModel()
        {
            ContentsBuilder();
        }

        private void ContentsBuilder()
        {
            Contents = new List<ContentsTree>()
            {
                new ContentsTree(){
                    ContentName = "コンテンツ",
                    Children = new List<ContentsTree>()
                    {
                        new ContentsTree(){ContentName = "カスタムコントロール"},
                        new ContentsTree(){ContentName = "レイアウト"},
                    },
                },
            };
        }
    }
}
