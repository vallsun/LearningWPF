using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common
{
    /// <summary>
    /// ビューモデルのベース。
    /// コマンドの初期化とプロパティの変更通知機能を持つ。
    /// </summary>
    public class ViewModelBase : BindableBase
    {
        #region プロパティ

        public virtual INotifyPropertyChanged Model { get; set; }

        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewModelBase(INotifyPropertyChanged model)
        {
            RegisterCommands();
            Model = model;
        }

        #endregion

        #region 初期化

        /// <summary>
        /// コマンドの初期化
        /// </summary>
        internal virtual void RegisterCommands()
        {

        }

        #endregion
    }
}
