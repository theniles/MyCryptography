using MyCryptographyApp.Models;
using MyCryptographyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Services
{
    internal class OperationService
    {
        public List<OperationViewModel> Operations { get; } = new List<OperationViewModel>();

        public void AddOperation(OperationViewModel operation)
        {
            Operations.Add(operation);
        }
    }
}
