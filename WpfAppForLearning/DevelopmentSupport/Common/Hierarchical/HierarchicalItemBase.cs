using System.Collections.ObjectModel;
using WPFAppFrameWork.Common;

namespace DevelopmentSupport.Common.Hierarchical
{
    public class HierarchicalItemBase<T> : BindableBase, IHierarchicalItem<T>
        where T : IHierarchicalItem<T>
    {
        public virtual T Parent { get; set; }
        public virtual ObservableCollection<T> Children { get; set; }
    }
}
