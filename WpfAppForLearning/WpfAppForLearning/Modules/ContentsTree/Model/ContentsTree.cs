using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAppForLearning.Modules.ContentsTree.Model
{
    /// <summary>
    /// コンテンツのツリーリスト
    /// </summary>
    public class ContentsTree
    {
        #region プロパティ

        public string ContentName { get; set; }
        public List<ContentsTree> Children { get; set; }

        #endregion

    }
}
