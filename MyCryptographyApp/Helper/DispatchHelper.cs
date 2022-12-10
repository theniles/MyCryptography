using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Helper
{
    internal static class DispatchHelper
    {
        public static async Task DispatchToUIAsync(Action action)
        {
            if (App.Current.Dispatcher.IsDispatchRequired)
                await App.Current.Dispatcher.DispatchAsync(action);
            else
                action();
        }
    }
}
