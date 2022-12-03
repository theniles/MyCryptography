using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class KeyOperationArgumentViewModel : OperationArgumentViewModel
    {

        private string m_TextKey;
        public string TextKey { get { return m_TextKey; } set { SetProperty(ref m_TextKey, value); } }


        private HashAlgorithmViewModel m_HashAlgorithm;
        public HashAlgorithmViewModel HashAlgorithm { get { return m_HashAlgorithm; } set { SetProperty(ref m_HashAlgorithm, value); } }



        private ObservableCollection<HashAlgorithmViewModel> m_MyBindableProperty;
        public ObservableCollection<HashAlgorithmViewModel> AvailableHashAlgorithms { get { return m_MyBindableProperty; } set { SetProperty(ref m_MyBindableProperty, value); } }

    }
}
