using MyCryptographyApp.Helper;
using MyCryptographyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.ViewModels
{
    internal class SymmetricCryptographyOperationViewModel : OperationViewModel
    {
        public SymmetricCryptographyOperationViewModel(string displayName, Func<SymmetricAlgorithm> symmetricAlgorithmFactory, Func<HashAlgorithm> hashAlgorithmFactory, SymmetricCryptographyOperationType operationType, string inputPath, string outputPath, byte[] unhashedBytes) : base(displayName)
        {
            SymmetricAlgorithmFactory = symmetricAlgorithmFactory;
            HashAlgorithmFactory = hashAlgorithmFactory;
            OperationType = operationType;
            InputPath = inputPath;
            OutputPath = outputPath;
            UnhashedKey = unhashedBytes;
            ProgressMessage = "Operation Added";
        }

        protected async override Task Execute()
        {
            await PermissionHelper.EnsurePermissionAsync<Permissions.StorageRead>();
            await PermissionHelper.EnsurePermissionAsync<Permissions.StorageWrite>();

            using (var inFile = new FileStream(InputPath, FileMode.Open, FileAccess.Read))
            {
                using (var outFile = new FileStream(OutputPath, FileMode.Create, FileAccess.Write))
                {
                    using (var algorithm = SymmetricAlgorithmFactory())
                    {
                        using (var hash = HashAlgorithmFactory())
                        {
                            var key = hash.ComputeHash(UnhashedKey);

                            algorithm.Key = key;

                            switch (OperationType)
                            {
                                case SymmetricCryptographyOperationType.Encryption:
                                    algorithm.GenerateIV();
                                    await outFile.WriteAsync(algorithm.IV, 0, algorithm.IV.Length);
                                    break;
                                case SymmetricCryptographyOperationType.Decryption:
                                    var ivBuffer = new byte[algorithm.BlockSize / 8];
                                    await inFile.ReadAsync(ivBuffer, 0, ivBuffer.Length);
                                    algorithm.IV = ivBuffer;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }

                            algorithm.Padding = PaddingMode.PKCS7;

                            var transform = OperationType switch
                            {
                                SymmetricCryptographyOperationType.Encryption => algorithm.CreateEncryptor(),
                                SymmetricCryptographyOperationType.Decryption => algorithm.CreateDecryptor(),
                                _ => throw new NotImplementedException()
                            };

                            var readBuffer = new byte[transform.InputBlockSize];
                            var transformBuffer = new byte[transform.OutputBlockSize];

                            const long progressInterval = 64;

                            long progressTick = 0;

                            while (inFile.Position < inFile.Length)
                            {
                                var readCount = await inFile.ReadAsync(readBuffer, 0, transform.InputBlockSize, CancellationTokenSource.Token);
                                if(readCount == transform.InputBlockSize)
                                {
                                    var transformedCount = transform.TransformBlock(readBuffer, 0, readCount, transformBuffer, 0);
                                    await outFile.WriteAsync(transformBuffer, 0, transformedCount, CancellationTokenSource.Token);
                                }
                                else
                                {
                                    var finalBlock = transform.TransformFinalBlock(readBuffer, 0, readCount);
                                    await outFile.WriteAsync(finalBlock, 0, finalBlock.Length, CancellationTokenSource.Token);
                                }
                                if(progressTick++ == progressInterval)
                                {
                                    await ProgressReportable.ReportProgressAsync((double)inFile.Position / inFile.Length);
                                    progressTick = 0;
                                }
                                CancellationTokenSource.Token.ThrowIfCancellationRequested();
                            }

                            await outFile.FlushAsync();
                            await ProgressReportable.ReportProgressAsync(1);
                        }
                    }
                }
            }
        }

        protected override OperationViewModel CreateReverseOperation()
        {
            var reversedOperationType = CryptographyOperationTypeHelper.ReverseType(OperationType);
            var prefix = StringHelper.GetSymmetricCryptographyOperationTypePrefix(reversedOperationType);
            var fi = new FileInfo(OutputPath);
            
            return new SymmetricCryptographyOperationViewModel($"{Name} Reversed", SymmetricAlgorithmFactory, HashAlgorithmFactory, reversedOperationType, OutputPath, $"{fi.DirectoryName}{Path.DirectorySeparatorChar}{prefix}{fi.Name}", UnhashedKey);
        }

        public Func<SymmetricAlgorithm> SymmetricAlgorithmFactory { get; }

        public Func<HashAlgorithm> HashAlgorithmFactory { get; }

        public SymmetricCryptographyOperationType OperationType { get; }

        public string InputPath { get; }

        public string OutputPath { get; }

        public byte[] UnhashedKey { get; }
    }
}
