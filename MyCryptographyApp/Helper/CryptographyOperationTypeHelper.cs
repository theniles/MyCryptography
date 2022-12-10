using MyCryptographyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Helper
{
    internal static class CryptographyOperationTypeHelper
    {
        public static SymmetricCryptographyOperationType ReverseType(SymmetricCryptographyOperationType type)
        {
            return type == SymmetricCryptographyOperationType.Encryption ? 
                   SymmetricCryptographyOperationType.Decryption :
                   SymmetricCryptographyOperationType.Encryption;
        }
    }
}
