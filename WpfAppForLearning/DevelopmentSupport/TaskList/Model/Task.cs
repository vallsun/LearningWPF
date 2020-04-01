using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.TaskList.Model
{
    /// <summary>
    /// タスク
    /// </summary>
    public class Task
    {
        #region プロパティ

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 登録日
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 完了日
        /// </summary>
        public DateTime DeadLine { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// 詳細
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; set; }

        #endregion
    }

    /// <summary>
    /// タスクのステータス
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 未着手
        /// </summary>
        NotStarted,
        
        /// <summary>
        /// 対応中
        /// </summary>
        InProgress,

        /// <summary>
        /// 停止
        /// </summary>
        Suspend,

        /// <summary>
        /// 完了
        /// </summary>
        Done
    }
}
