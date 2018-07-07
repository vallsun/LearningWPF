using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppForLearning.Modules.ContentsTree.Model
{
    /// <summary>
    /// コンテンツのツリーリスト
    /// </summary>
    public class Content
    {
        /// <summary>
        /// 親要素
        /// </summary>
        private Content m_Parent = null;
        /// <summary>
        /// 子要素のコレクション
        /// </summary>
        private ObservableCollection<Content> m_Children = new ObservableCollection<Content>();

        #region プロパティ

        /// <summary>
        /// 名前
        /// </summary>
        public string ContentName { get; set; }
        /// <summary>
        /// 親
        /// </summary>
        public Content Parent {
            get
            {
                return m_Parent;
            }
            set
            {
                m_Parent = value;
            }
        }
        /// <summary>
        /// 子
        /// </summary>
        public ObservableCollection<Content> Children
        {
            get
            {
                return m_Children;
            }
            set
            {
                m_Children = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent"></param>
        public Content(Content parent)
        {
            if (parent == null)
            {
                return;
            }
            Parent = parent;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Content()
        {
            return;
        }


        /// <summary>
        /// 子要素の追加
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(Content child)
        {
            if(child == null)
            {
                return;
            }
            Children.Add(child);
            child.Parent = this;
        }

        #endregion

    }

    /// <summary>
    /// コンテンツ
    /// </summary>
    public class Contents : TreeView
    {

        /// <summary>
        /// コンテンツ
        /// </summary>
        public ObservableCollection<Content> ContentsTree { get; set; }

        public Contents()
        {
            SelectedItemChanged += OnSelectedItemChanged;
            ContentsBuilder();
        }

        public void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            return;
        }

        /// <summary>
        /// コンテンツの生成
        /// </summary>
        private void ContentsBuilder()
        {
            ContentsTree = new ObservableCollection<Content>();
            var rootContent = new Content()
            {
                ContentName = "コンテンツ",
            };

            var customControlCotent = new Content()
            {
                ContentName = "カスタムコントロール",
            };

            var layoutContent = new Content()
            {
                ContentName = "レイアウト",
            };
            rootContent.AddChild(customControlCotent);
            rootContent.AddChild(layoutContent);
            ContentsTree.Add(rootContent);
        }
    }
}
