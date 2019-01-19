using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfAppForLearning.Properties;

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
        private ObservableCollection<Content> m_Children = null;

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

        #endregion

        #region 構築・消滅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent"></param>
        public Content()
        {
        }

        #endregion

        #region 公開メソッド

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
    public class Contents
    {
        /// <summary>
        /// コンテンツ
        /// </summary>
        public ObservableCollection<Content> ContentsTree { get; set; }

        public Contents()
        {
            ContentsBuilder();
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
                Children = new ObservableCollection<Content>(),
            };

            var controlCotent = new Content()
            {
                ContentName = Strings.ContentName_Control,
                Children = new ObservableCollection<Content>(),
            };

            var ProgressBarContent = new Content()
            {
                ContentName = Strings.ContentName_ProgressBar,
            };

            var userControlCotent = new Content()
            {
                ContentName = Strings.ContentName_UserControl,
            };

            var customControlCotent = new Content()
            {
                ContentName = Strings.ContentName_CustomControl,
            };

            var bindingCotent = new Content()
            {
                ContentName = "バインディング",
            };

            var converterCotent = new Content()
            {
                ContentName = "コンバータ",
            };

            var keyboardNavigationContent = new Content()
            {
                ContentName = "KeyboardNavigation"
            };

            var layoutContent = new Content()
            {
                ContentName = "レイアウト",
                Children = new ObservableCollection<Content>(),
            };

            var panelContent = new Content()
            {
                ContentName = "パネル(Panel)",
                Children = new ObservableCollection<Content>(),
            };

            var stackPanelContent = new Content()
            {
                ContentName = "StackPanel",
            };

            var dockPanelContent = new Content()
            {
                ContentName = "DockPanel",
            };

            var wrapPanelContent = new Content()
            {
                ContentName = "WrapPanel",
            };

            var CanvasContent = new Content()
            {
                ContentName = "Canvas",
            };

            //レイアウト
            layoutContent.AddChild(panelContent);
            panelContent.AddChild(stackPanelContent);
            panelContent.AddChild(dockPanelContent);
            panelContent.AddChild(wrapPanelContent);
            panelContent.AddChild(CanvasContent);

            //コントロール
            controlCotent.AddChild(ProgressBarContent);

            rootContent.AddChild(controlCotent);
            rootContent.AddChild(userControlCotent);
            rootContent.AddChild(customControlCotent);
            rootContent.AddChild(bindingCotent);
            rootContent.AddChild(keyboardNavigationContent);
            rootContent.AddChild(layoutContent);
            ContentsTree.Add(rootContent);
        }
    }
}
