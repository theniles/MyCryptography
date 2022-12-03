using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal interface IProgressReportable
    {
        public Task ReportProgressAsync(double progress);

        public Task ReportErrorAsync(string error);
    }
}
