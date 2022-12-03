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
        public string DisplayName { get { return m_Name; } set { SetProperty(ref m_Name, value); } }

        public Func<HashAlgorithm> HashFactory { get; }

        public HashAlgorithmViewModel(string displayName, Func<HashAlgorithm> hashFactory)
        {
            DisplayName = displayName;
            HashFactory = hashFactory;
        }

        public HashAlgorithm NewAlgorithm => HashFactory();

    }
}
