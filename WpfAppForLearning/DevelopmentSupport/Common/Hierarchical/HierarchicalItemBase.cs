using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common.Hierarchical
{
    public class HierarchicalItemBase<T> : BindableBase, IHierarchicalItem<T>
        where T : IHierarchicalItem<T>
    {
        public virtual T Parent { get; set; }
        public virtual ObservableCollection<T> Children { get; set; }
    }
}
