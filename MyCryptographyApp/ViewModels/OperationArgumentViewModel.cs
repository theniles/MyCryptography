using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal abstract class OperationArgumentViewModel : ObservableViewModelBase
    {
        private string m_Name;
        public string Name { get { return m_Name; } set { SetProperty(ref m_Name, value); } }
    }
}
