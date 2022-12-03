using MyCryptographyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCryptographyApp.ViewModels
{
    internal class OperationViewModel : ObservableViewModelBase, IProgressReportable
    {
        private DateTime m_StartTime;
        public DateTime StartTime { get { return m_StartTime; } set { SetProperty(ref m_StartTime, value); } }

        private DateTime m_EndTime;
        public DateTime EndTime { get { return m_EndTime; } set { SetProperty(ref m_EndTime, value); } }

        private double m_Progress;
        public double Progress { get { return m_Progress; } set { SetProperty(ref m_Progress, value); } }

        private string m_DisplayName;

        public OperationViewModel(string displayName, Action<IProgressReportable, CancellationToken> action)
        {
            DisplayName = displayName;

            CancellationTokenSource = new CancellationTokenSource();

            CancelCommand = new Command(() =>
            {
                CancellationTokenSource.Cancel();
            },
            ()=>!Task.IsCompleted);

            StartTime = DateTime.Now;

            Task = Task.Run(() => { action(this, CancellationTokenSource.Token); }).ContinueWith(
                (task) => 
                {
                    CancellationTokenSource.Dispose();
                    if (App.Current.Dispatcher.IsDispatchRequired)
                        App.Current.Dispatcher.Dispatch(() => EndTime = DateTime.Now);
                    else
                        EndTime = DateTime.Now;
                });
        }

        public string DisplayName { get { return m_DisplayName; } set { SetProperty(ref m_DisplayName, value); } }


        private string m_ProgressMessage;
        public string ProgressMessage { get { return m_ProgressMessage; } set { SetProperty(ref m_ProgressMessage, value); } }


        public Task Task { get; }

        public CancellationTokenSource CancellationTokenSource { get; }

        public ICommand CancelCommand { get; }

        public async Task ReportProgressAsync(double progress)
        {
            if (App.Current.Dispatcher.IsDispatchRequired)
                await App.Current.Dispatcher.DispatchAsync(() => Progress = progress);
            else
                Progress = progress;
        }

        public async Task ReportErrorAsync(string error)
        {
            if (App.Current.Dispatcher.IsDispatchRequired)
                await App.Current.Dispatcher.DispatchAsync(() => ProgressMessage = error);
            else
                ProgressMessage = error;
        }
    }
}
