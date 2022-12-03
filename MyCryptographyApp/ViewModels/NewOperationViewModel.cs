using MyCryptographyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCryptographyApp.ViewModels
{
    internal class NewOperationViewModel : ObservableViewModelBase
    {
        private ObservableCollection<OperationTypeViewModel> m_OperationTypes;
        public ObservableCollection<OperationTypeViewModel> OperationTypes { get { return m_OperationTypes; } set { SetProperty(ref m_OperationTypes, value); } }

        private OperationTypeViewModel m_SelectedOperationType;

        private readonly OperationService operationService;

        public OperationTypeViewModel SelectedOperationType { get { return m_SelectedOperationType; } set { SetProperty(ref m_SelectedOperationType, value); } }

        public ICommand ExecuteOperation { get; }

        public NewOperationViewModel()
        {
            operationService = App.Current.Handler.MauiContext.Services.GetService<OperationService>();

            OperationTypes = new ObservableCollection<OperationTypeViewModel>()
            {
                new OperationTypeViewModel(){Name="AES", Arguments = new ObservableCollection<OperationArgumentViewModel>()
                {
                    new PathOperationArgumentViewModel(){Name="AInput path"},
                    new PathOperationArgumentViewModel(){Name="AOutput path"}

                }},
                new OperationTypeViewModel(){Name="DES", Arguments = new ObservableCollection<OperationArgumentViewModel>()
                {
                    new PathOperationArgumentViewModel(){Name="DInput path"},
                    new PathOperationArgumentViewModel(){Name="DOutput path"},
                    new KeyOperationArgumentViewModel(){Name="Key", AvailableHashAlgorithms = new ObservableCollection<HashAlgorithmViewModel>()
                    { 
                        new HashAlgorithmViewModel(){Name="None"},
                        new HashAlgorithmViewModel(){Name="SHA256"},

                    }}
                }} 
            };

            ExecuteOperation = new Command(() =>
            {
                SelectedOperationType
            });
        }
    }
}
