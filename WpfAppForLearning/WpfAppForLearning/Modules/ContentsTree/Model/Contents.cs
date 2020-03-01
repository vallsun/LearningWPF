using DevelopmentSupport.Common.Hierarchical;
using DevelopmentSupport.Common.Namable;
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
    public class Content : INamable, IHierarchicalItem
    {
        /// <summary>
        /// 親要素
        /// </summary>
        private IHierarchicalItem m_Parent = null;

        /// <summary>
        /// 子要素のコレクション
        /// </summary>
        private ObservableCollection<IHierarchicalItem> m_Children = null;

        #region プロパティ

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 親
        /// </summary>
        public IHierarchicalItem Parent
        {
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
        public ObservableCollection<IHierarchicalItem> Children
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
                Name = "コンテンツ",
                Children = new ObservableCollection<IHierarchicalItem>(),
            };

            var controlCotent = new Content()
            {
                Name = Strings.ContentName_Control,
                Children = new ObservableCollection<IHierarchicalItem>(),
            };

            var ProgressBarContent = new Content()
            {
                Name = Strings.ContentName_ProgressBar,
            };

            var userControlCotent = new Content()
            {
                Name = Strings.ContentName_UserControl,
            };

            var customControlCotent = new Content()
            {
                Name = Strings.ContentName_CustomControl,
            };

            var bindingCotent = new Content()
            {
                Name = "バインディング",
            };

            var converterCotent = new Content()
            {
                Name = "コンバータ",
            };

            var keyboardNavigationContent = new Content()
            {
                Name = "KeyboardNavigation"
            };

            var dragDropControlContent = new Content()
            {
                Name = Strings.ContentName_DragDropControl,
            };

            var layoutContent = new Content()
            {
                Name = "レイアウト",
                Children = new ObservableCollection<IHierarchicalItem>(),
            };

            var panelContent = new Content()
            {
                Name = "パネル(Panel)",
                Children = new ObservableCollection<IHierarchicalItem>(),
            };

            var stackPanelContent = new Content()
            {
                Name = "StackPanel",
            };

            var dockPanelContent = new Content()
            {
                Name = "DockPanel",
            };

            var wrapPanelContent = new Content()
            {
                Name = "WrapPanel",
            };

            var CanvasContent = new Content()
            {
                Name = "Canvas",
            };

            var TextBoxContent = new Content()
            {
                Name = "TextBox",
            };

            //レイアウト
            layoutContent.AddChild(panelContent);
            panelContent.AddChild(stackPanelContent);
            panelContent.AddChild(dockPanelContent);
            panelContent.AddChild(wrapPanelContent);
            panelContent.AddChild(CanvasContent);

            //コントロール
            controlCotent.AddChild(ProgressBarContent);
            controlCotent.AddChild(TextBoxContent);

            rootContent.AddChild(controlCotent);
            rootContent.AddChild(userControlCotent);
            rootContent.AddChild(customControlCotent);
            rootContent.AddChild(bindingCotent);
            rootContent.AddChild(keyboardNavigationContent);
            rootContent.AddChild(dragDropControlContent);
            rootContent.AddChild(layoutContent);
            ContentsTree.Add(rootContent);
        }
    }
}
