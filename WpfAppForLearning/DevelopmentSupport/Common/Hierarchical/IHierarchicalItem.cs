using DevelopmentSupport.Common.Namable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common.Hierarchical
{
    /// <summary>
    /// 階層構造を持つ要素
    /// </summary>
    public interface IHierarchicalItem<T>
        where T : IHierarchicalItem<T>
    {
        #region プロパティ

        /// <summary>
        /// 親要素
        /// </summary>
       T Parent { get; set; }

        /// <summary>
        /// 子要素群
        /// </summary>
        ObservableCollection<T> Children { get; set; }

        #endregion
    }
}
