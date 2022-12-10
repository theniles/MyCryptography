using MyCryptographyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Helper
{
    internal static class StringHelper
    {
        public static string GetSymmetricCryptographyOperationTypePrefix(SymmetricCryptographyOperationType type)
        {
            return type == SymmetricCryptographyOperationType.Encryption ? "ENCRYPTED" : "DECRYPTED";
        }
    }
}
