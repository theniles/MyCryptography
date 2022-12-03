using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class SymmetricAlgorithmViewModel : ObservableViewModelBase
    {

        private string m_DisplayName;

        public SymmetricAlgorithmViewModel(string displayName, Func<SymmetricAlgorithm> algorithmFactory)
        {
            DisplayName = displayName;
            AlgorithmFactory = algorithmFactory;
            SymmetricAlgorithm a;
        }

        public string DisplayName { get { return m_DisplayName; } set { SetProperty(ref m_DisplayName, value); } }

        public Func<SymmetricAlgorithm> AlgorithmFactory { get; }

        public SymmetricAlgorithm NewAlgorithm => AlgorithmFactory();
    }
}
