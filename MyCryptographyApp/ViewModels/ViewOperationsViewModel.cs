using MyCryptographyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class ViewOperationsViewModel : ObservableViewModelBase
    {
        public OperationService OperationService { get; } 

        public ViewOperationsViewModel()
        {
            OperationService = App.Current.Handler.MauiContext.Services.GetRequiredService<OperationService>();
        }


    }
}
