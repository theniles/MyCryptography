using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Models
{
    internal abstract class OperationModel
    {
        public Task Task { get; protected set; }
    }
}
