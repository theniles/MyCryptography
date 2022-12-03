using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class OperationViewModel : ObservableViewModelBase
    {
        private DateTime m_StartTime;
        public DateTime StartTime { get { return m_StartTime; } set { SetProperty(ref m_StartTime, value); } }

        private DateTime m_EndTime;
        public DateTime EndTime { get { return m_EndTime; } set { SetProperty(ref m_EndTime, value); } }


        private string m_TypeName;
        public string TypeName { get { return m_TypeName; } set { SetProperty(ref m_TypeName, value); } }


        private string m_GivenName;
        public string GivenName { get { return m_GivenName; } set { SetProperty(ref m_GivenName, value); } }

        private double m_Progress;
        public double Progress { get { return m_Progress; } set { SetProperty(ref m_Progress, value); } }

        private ObservableCollection<OperationArgumentViewModel> m_Arguments;

        public OperationViewModel(Task task)
        {

        }

        public ObservableCollection<OperationArgumentViewModel> Arguments { get { return m_Arguments; } set { SetProperty(ref m_Arguments, value); } }

    }
}
