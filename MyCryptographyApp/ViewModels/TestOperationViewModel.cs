using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class TestOperationViewModel : OperationViewModel
    {
        public TestOperationViewModel(string displayName) : base(displayName)
        {
        }

        protected override OperationViewModel CreateReverseOperation()
        {
            throw new NotImplementedException();
        }

        protected override async Task Execute()
        {
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(1000);
                await ProgressReportable.ReportProgressAsync(i / 10d);
            }
        }
    }
}
