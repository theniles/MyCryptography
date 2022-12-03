using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class HashAlgorithmViewModel : ObservableViewModelBase
    {

        private string m_Name;
        public string Name { get { return m_Name; } set { SetProperty(ref m_Name, value); } }


        private HashAlgorithm m_MyBindableProperty;
        public HashAlgorithm HashAlgorithm { get { return m_MyBindableProperty; } set { SetProperty(ref m_MyBindableProperty, value); } }

    }
}
