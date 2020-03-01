using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common.Namable
{
    /// <summary>
    /// 名前付け可能な要素
    /// </summary>
    public interface INamable
    {
        #region プロパティ

        /// <summary>
        /// 名前
        /// </summary>
        string Name { get; set; }

        #endregion
    }
}
