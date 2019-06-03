using DevelopmentSupport.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Setting
{
    public class Setting :BindableBase
    {
        private string m_Name;
        private ObservableCollection<Setting> m_Children;


        public string Name { get { return m_Name; } set { SetProperty(ref m_Name, value); } }
        public ObservableCollection<Setting> Children { get { return m_Children; } set { SetProperty(ref m_Children, value); } }
    }
}
