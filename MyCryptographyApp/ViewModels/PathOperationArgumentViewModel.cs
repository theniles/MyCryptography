using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class PathOperationArgumentViewModel : OperationArgumentViewModel
    {

        private string m_Path;
        public string Path { get { return m_Path; } set { SetProperty(ref m_Path, value); } }

    }
}
