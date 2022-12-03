using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class SymmetricOperationType : OperationTypeViewModel
    {
        public override OperationViewModel CreateOperation()
        {
            return new OperationViewModel()
            {

            }
        }
    }
}
