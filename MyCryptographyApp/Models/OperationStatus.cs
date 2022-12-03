using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Models
{
    internal enum OperationStatus
    {
        Ready,
        Running,
        Completed,
        Cancelled,
        Failed
    }
}
