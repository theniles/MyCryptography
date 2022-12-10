using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Helper
{
    internal static class PermissionHelper
    {
        public static async Task EnsurePermissionAsync<T>() where T : Permissions.BasePermission, new()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<T>();

            if (status == PermissionStatus.Granted)
                return;

            status = await Permissions.RequestAsync<T>();

            if (status != PermissionStatus.Granted)
                throw new Exception($"Permission {typeof(T).Name} {status}");
        }
    }
}
