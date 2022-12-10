using MyCryptographyApp.Helper;
using MyCryptographyApp.Models;
using MyCryptographyApp.Services;
using System;
using System.Windows.Input;

namespace MyCryptographyApp.ViewModels
{
    internal abstract class OperationViewModel : ObservableViewModelBase, IProgressReportable
    {
        protected OperationViewModel(string displayName)
        {
            OperationService = App.Current.Handler.MauiContext.Services.GetRequiredService<OperationService>();
            ProgressReportable = this;
            Status = OperationStatus.Ready;
            Name = displayName;
            StartTime = DateTime.Now;
            EndTime = DateTime.UnixEpoch;

            CancelCommand = new Command(() =>
            {
                if(Status == OperationStatus.Running)
                    CancellationTokenSource.Cancel();
            }, () => Status == OperationStatus.Running);

            DismissCommand = new Command(() =>
            {
                OperationService.DismissOperation(this);
            }, () => Status != OperationStatus.Running);

            RunCommand = new Command(async () =>
            {
                if(Status != OperationStatus.Running)
                    await Run();
            }, ()=> Status != OperationStatus.Running);

            ReverseCommand = new Command(() =>
            {
                if(Status != OperationStatus.Running)
                    OperationService.AddOperation(CreateReverseOperation());
            }, () => Status != OperationStatus.Running);
        }

        protected abstract OperationViewModel CreateReverseOperation();

        public async Task Run()
        {
            if (Status != OperationStatus.Ready)
                throw new InvalidOperationException();
            try
            {
                Status = OperationStatus.Running;
                CancellationTokenSource = new CancellationTokenSource();
                StartTime = DateTime.Now;
                Task = Task.Run(Execute, CancellationTokenSource.Token);
                await ReportMessageAsync("Operation Running");
                await ReportProgressAsync(0);
                await Task;
                Status = OperationStatus.Completed;
                await ReportMessageAsync("Operation Complete");
            }
            catch (TaskCanceledException)
            {
                Status = OperationStatus.Cancelled;
                await ReportMessageAsync("Operation cancelled");
            }
            catch (Exception ex)
            {
                Status = OperationStatus.Failed;
                Exception = ex;
                await ReportMessageAsync($"Operation failed. {ex.Message}");
            }
            finally
            {
                EndTime = DateTime.Now;
                CancellationTokenSource.Dispose();
                Task.Dispose();
                Status = OperationStatus.Ready;
            }
        }

        public OperationService OperationService { get; }

        public ICommand CancelCommand { get; }

        public ICommand DismissCommand { get; }

        public ICommand RunCommand { get; }

        public ICommand ReverseCommand { get; }

        private OperationStatus m_Status;
        public OperationStatus Status { get { return m_Status; } set { SetProperty(ref m_Status, value); } }

        private DateTime m_StartTime;
        public DateTime StartTime { get { return m_StartTime; } set { SetProperty(ref m_StartTime, value); } }

        private DateTime m_EndTime;
        public DateTime EndTime { get { return m_EndTime; } set { SetProperty(ref m_EndTime, value); } }

        private string m_Name;
        public string Name { get { return m_Name; } set { SetProperty(ref m_Name, value); } }

        private double m_Progress;
        public double Progress { get { return m_Progress; } set { SetProperty(ref m_Progress, value); } }

        private string m_ProgressMessage;
        public string ProgressMessage { get { return m_ProgressMessage; } set { SetProperty(ref m_ProgressMessage, value); } }

        private Task Task { get; set; }

        public Exception Exception { get; private set; }

        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public IProgressReportable ProgressReportable { get; private set; }

        protected abstract Task Execute();

        public async Task ReportProgressAsync(double progress)
        {
            await DispatchHelper.DispatchToUIAsync(() => Progress = progress);
        }

        public async Task ReportMessageAsync(string message)
        {
            await DispatchHelper.DispatchToUIAsync(() => ProgressMessage = message);
        }
    }
}