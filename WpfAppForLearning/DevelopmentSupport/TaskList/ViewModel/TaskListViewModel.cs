using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevelopmentSupport.Common;
using Task = DevelopmentSupport.TaskList.Model.Task;
using TaskStatus = DevelopmentSupport.TaskList.Model.TaskStatus;

namespace DevelopmentSupport.TaskList.ViewModel
{
    /// <summary>
    /// タスクリストのVM
    /// </summary>
    class TaskListViewModel : ViewModelBase
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
        public ICommand AddTaskCommand { get; }

        /// <summary>
        /// タスクを削除
        /// </summary>
        public ICommand RemoveTaskCommand { get; }

        /// <summary>
        /// ステータスを更新
        /// </summary>
        public ICommand UpdateStatusCommand { get; }

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

        internal override void RegisterCommands()
        {
            base.RegisterCommands();

        }

        #endregion
    }
}
