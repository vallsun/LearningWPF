using System;
using System.Windows.Input;

namespace DevelopmentSupport.Common
{
    #region No parameter DelegateCommand

    /// <summary>
    /// 引数無しのコマンドクラス
    /// </summary>
    public sealed class DelegateCommand : ICommand
    {

        #region フィールド

        private Action _execute;
        private Func<bool> _canExecute;

        #endregion

        #region 構築・消滅

        /// <summary>
        /// Execute処理のみを指定された場合のコンストラクタ
        /// CanExecute処理は常にtrueを返す
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action execute) : this(execute, () => true)
        {
        }

        /// <summary>
        /// Execute処理とCanExecute処理を指定された場合のコンストラクタ
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        /// <summary>
        /// コマンド実行可否判定処理
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return _canExecute();
        }

        /// <summary>
        /// コマンド実行処理
        /// </summary>
        public void Execute()
        {
            _execute();
        }

        /// <summary>
        /// イベントハンドラ
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #region ICommandメンバ

        /// <summary>
        /// ICommandインターフェースの実装(CanExecute)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// ICommandインターフェースの実装(Execute)
        /// </summary>
        /// <param name="parameter"></param>
        void ICommand.Execute(object parameter)
        {
            Execute();
        }
        #endregion
    }
    #endregion

    #region Parameter DelegateCommand

    /// <summary>
    /// 引数ありのコマンドクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Func<T, bool> _canExecute;

        private static readonly bool IS_VALUE_TYPE;

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static DelegateCommand()
        {
            IS_VALUE_TYPE = typeof(T).IsValueType;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action<T> execute) : this(execute, o => true)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        public virtual bool CanExecute(T parameter)
        {
            return _canExecute(parameter);
        }

        public virtual void Execute(T parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #region ICommandメンバ

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(Cast(parameter));
        }

        void ICommand.Execute(object parameter)
        {
            Execute(Cast(parameter));
        }
        #endregion

        /// <summary>
        /// パラメータ値を変換する
        /// </summary>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private T Cast(object parameter)
        {
            if (parameter == null && IS_VALUE_TYPE)
            {
                return default(T);
            }
            return (T)parameter;
        }
    }
    #endregion
}
