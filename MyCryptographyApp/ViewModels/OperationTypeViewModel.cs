using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal abstract class OperationTypeViewModel : ObservableViewModelBase
    {

        private string m_OperationName;
        public string Name { get { return m_OperationName; } set { SetProperty(ref m_OperationName, value); } }

        private ObservableCollection<OperationArgumentViewModel> m_Arguments;
        public ObservableCollection<OperationArgumentViewModel> Arguments { get { return m_Arguments; } set { SetProperty(ref m_Arguments, value); } }

        public abstract OperationViewModel CreateOperation();
    }
}
