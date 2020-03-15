using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common.Selectable
{
    /// <summary>
    /// 要素選択可能なクラスのインターフェース
    /// </summary>
    public interface ISelectable<T>
    {
        /// <summary>
        /// 選択中の要素
        /// </summary>
        T SelectedItem { get; set; }
    }
}
