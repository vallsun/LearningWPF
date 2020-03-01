using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common
{
    public class ViewModelBase : BindableBase
    {
        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewModelBase()
        {
            RegisterCommands();
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
