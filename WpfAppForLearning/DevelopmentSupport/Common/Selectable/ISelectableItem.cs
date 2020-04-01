﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common.Selectable
{
    /// <summary>
    /// 選択可能な要素のインターフェース
    /// </summary>
    public interface ISelectableItem
    {
        #region プロパティ

        /// <summary>
        /// 選択されているか
        /// </summary>
        bool IsSelected { get; set; }

        #endregion
    }
}