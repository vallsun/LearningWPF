using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common.Selectable
{
    /// <summary>
    /// 選択要素を保持することが可能なVM
    /// </summary>
    public class SelectableViewModelBase<T> : ViewModelBase, ISelectable<T>
        where T : ISelectableItem
    {
        #region フィールド

        /// <summary>
        /// 選択された要素
        /// </summary>
        private T m_SelectedItem;

        /// <summary>
        /// 選択された要素群
        /// </summary>
        private IEnumerable<T> m_SelectedItems;

        #endregion

        #region プロパティ

        /// <summary>
        /// 選択中の要素
        /// </summary>
        public T SelectedItem
        {
            get { return m_SelectedItem; }
            set
            {
                if(m_SelectedItem != null)
                {
                    m_SelectedItem.IsSelected = false;
                }
                SetProperty(ref m_SelectedItem, value);
                if (m_SelectedItem != null)
                {
                    m_SelectedItem.IsSelected = true;
                }
            }
        }

        public IEnumerable<T> SelectedItems
        {
            get { return m_SelectedItems; }
            set { SetProperty(ref m_SelectedItems, value); }
        }

        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectableViewModelBase(INotifyPropertyChanged model)
            : base(model)
        {
            PropertyChanged += OnSelectedItemChanged;
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// 選択中のアイテムが変更された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnSelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {

        }
        
        #endregion

    }
}
