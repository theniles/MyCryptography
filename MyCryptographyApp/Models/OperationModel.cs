using MyCryptographyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Models
{
    internal class OperationModel : IDisposable
    {
        public OperationModel(Action<IProgressReportable, CancellationToken> action, IProgressReportable progress)
        {
            CancellationTokenSource = new CancellationTokenSource();
            Task = Task.Run(() => { action(progress, CancellationTokenSource.Token); });
        }

        public Task Task { get; }

        public CancellationTokenSource CancellationTokenSource { get; }

        public void Dispose()
        {
            ((IDisposable)CancellationTokenSource).Dispose();
        }
    }
}
