using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevelopmentSupport.Common;
using DevelopmentSupport.Common.Selectable;
using Task = DevelopmentSupport.TaskList.Model.Task;
using TaskStatus = DevelopmentSupport.TaskList.Model.TaskStatus;

namespace DevelopmentSupport.TaskList.ViewModel
{
    /// <summary>
    /// タスクリストのVM
    /// </summary>
    public class TaskListViewModel : SelectableViewModelBase<Task>
    {
        #region プロパティ

        /// <summary>
        /// タスクのリスト
        /// </summary>
        public List<Task> Tasks { get; set; }

        #region コマンド

        /// <summary>
        /// タスクを追加
        /// </summary>
        public ICommand AddTaskCommand { get; private set; }

        /// <summary>
        /// タスクを削除
        /// </summary>
        public ICommand RemoveTaskCommand { get; private set; }

        /// <summary>
        /// ステータスを更新
        /// </summary>
        public ICommand UpdateStatusCommand { get; private set; }

        #endregion

        #endregion

        #region 構築消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskListViewModel()
        :base(null)
        {
            Tasks = new List<Task>();
            Tasks.Add(new Task()
            {
                Title = "SAmple1",
                Description = "Description1",
                RegisterDate = DateTime.Today,
                DeadLine = DateTime.Parse("2020/4/5"),
                Status = TaskStatus.NotStarted
            });
        }

        #endregion

        #region 初期化

        /// <summary>
        /// コマンドの登録
        /// </summary>
        public override void RegisterCommands()
        {
            base.RegisterCommands();
            RemoveTaskCommand = new DelegateCommand(RemoveTask, CanRemoveTask);

        }

        #endregion

        #region タスクの削除

        /// <summary>
        /// タスクを削除する
        /// </summary>
        private void RemoveTask()
        {
            // TODO: SelectedItemsはViewとのバインディングが未実装のため、
            // SelectedItemが存在するかのみを確認してタスクを削除する。
            if (SelectedItem == null)
            {
                return;
            }

            Tasks.Remove(SelectedItem);
        }

        /// <summary>
        /// タスクを削除できるか
        /// </summary>
        /// <returns></returns>
        private bool CanRemoveTask()
        {
            // TODO: SelectedItemsはViewとのバインディングが未実装のため、
            // SelectedItemが存在するかのみを確認してタスクを削除する。
            return SelectedItem != null;
        }

        #endregion
    }
}
