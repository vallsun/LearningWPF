using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common.Selectable
{
    /// <summary>
    /// 選択要素を保持することが可能なVM
    /// </summary>
    public class SelectableViewModelBase<T> : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 選択された要素
        /// </summary>
        private T m_SelectedItem;

        #endregion

        #region プロパティ

        /// <summary>
        /// 選択中の要素
        /// </summary>
        public T SelectedItem
        {
            get { return m_SelectedItem; }
            set { SetProperty(ref m_SelectedItem, value); }
        }

        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectableViewModelBase()
        {

        }
        #endregion
    }
}
