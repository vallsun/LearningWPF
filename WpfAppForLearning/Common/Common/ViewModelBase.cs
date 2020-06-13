using System.ComponentModel;

namespace DevelopmentCommon.Common
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
        protected virtual void RegisterCommands()
        {
            // 派生先で実装
        }

        #endregion
    }
}
