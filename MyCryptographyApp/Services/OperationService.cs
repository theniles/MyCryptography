using MyCryptographyApp.Models;
using MyCryptographyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Services
{
    internal class OperationService
    {
        public ObservableCollection<OperationViewModel> Operations { get; } = new ObservableCollection<OperationViewModel>();

        public void AddOperation(OperationViewModel operation)
        {
            Operations.Add(operation);
        }

        public void DismissOperation(OperationViewModel operation)
        {
            Operations.Remove(operation);
        }
    }
}
