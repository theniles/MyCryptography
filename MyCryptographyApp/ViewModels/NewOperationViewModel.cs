using MyCryptographyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCryptographyApp.ViewModels
{
    internal class NewOperationViewModel : ObservableViewModelBase
    {
        public ICommand ExecuteOperation { get; }

        public List<HashAlgorithmViewModel> HashAlgorithms { get; }

        public List<SymmetricAlgorithmViewModel> SymmetricAlgorithms { get; }

        private HashAlgorithmViewModel m_SelectedHashAlgorithm;
        public HashAlgorithmViewModel SelectedHashAlgorithm { get { return m_SelectedHashAlgorithm; } set { SetProperty(ref m_SelectedHashAlgorithm, value); } }

        private SymmetricAlgorithmViewModel m_SelectedSymmetricAlgorithm;
        public SymmetricAlgorithmViewModel SelectedSymmetricAlgorithm { get { return m_SelectedSymmetricAlgorithm; } set { SetProperty(ref m_SelectedSymmetricAlgorithm, value); } }

        private string m_KeyString;
        public string KeyString { get { return m_KeyString; } set { SetProperty(ref m_KeyString, value); } }

        private bool m_Encrypt;
        public bool Encrypt { get { return m_Encrypt; } set { SetProperty(ref m_Encrypt, value); } }

        private string m_InputPath;
        public string InputPath { get { return m_InputPath; } set { SetProperty(ref m_InputPath, value); } }

        private string m_OutputPath;
        public string OutputPath { get { return m_OutputPath; } set { SetProperty(ref m_OutputPath, value); } }

        private OperationService OperationService { get; }

        public NewOperationViewModel()
        {
            HashAlgorithms = new List<HashAlgorithmViewModel>()
            {
                new HashAlgorithmViewModel("SHA256", ()=> SHA256.Create()),
                new HashAlgorithmViewModel("SHA512", ()=> SHA512.Create()),
                new HashAlgorithmViewModel("MD5", ()=> MD5.Create()),

            };

            SymmetricAlgorithms = new List<SymmetricAlgorithmViewModel>()
            {
                new SymmetricAlgorithmViewModel("AES", ()=>Aes.Create()),
                new SymmetricAlgorithmViewModel("Triple DES", ()=>TripleDES.Create()),

            };

            //SelectedHashAlgorithm = HashAlgorithms[0];
            //SelectedSymmetricAlgorithm = SymmetricAlgorithms[0];

            ExecuteOperation = new Command(async () =>
            {
                if(SelectedSymmetricAlgorithm == null || SelectedHashAlgorithm == null || KeyString == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please enter all details", "OK");
                    return;
                }
                var algorithm = SelectedSymmetricAlgorithm.NewAlgorithm;
                var hash = SelectedHashAlgorithm.NewAlgorithm;
                var key = hash.ComputeHash(Encoding.ASCII.GetBytes(KeyString));
                if(!algorithm.ValidKeySize(key.Length * 8))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Invalid key size", "OK");
                    return;
                }
                algorithm.Key = key;
                algorithm.GenerateIV();

                var transform = Encrypt ? algorithm.CreateEncryptor() : algorithm.CreateDecryptor();

                var inputPath = InputPath;
                var outputPath = OutputPath;

                OperationService.AddOperation(new OperationViewModel("Operation", async (p, ct) =>
                {
                    try
                    {
                        using (var inFile = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
                        {
                            using (var outFile = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                var readBuffer = new byte[transform.InputBlockSize];
                                var transformBuffer = new byte[transform.OutputBlockSize];

                                while (inFile.Position != inFile.Length)
                                {
                                    var readCount = await inFile.ReadAsync(readBuffer, 0, transform.InputBlockSize);
                                    if (readCount != transform.InputBlockSize)
                                    {
                                        var finalBlock = transform.TransformFinalBlock(readBuffer, 0, readCount);
                                        await outFile.WriteAsync(finalBlock, 0, finalBlock.Length);
                                    }
                                    else
                                    {
                                        transform.TransformBlock(readBuffer, 0, readBuffer.Length, transformBuffer, 0);
                                        await outFile.WriteAsync(transformBuffer, 0, transformBuffer.Length);
                                    }
                                    await p.ReportProgressAsync((double)inFile.Position / inFile.Length);
                                    ct.ThrowIfCancellationRequested();
                                }

                                await p.ReportProgressAsync(1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await p.ReportErrorAsync(ex.Message);
                    }
                }));
            });

            OperationService = App.Current.Handler.MauiContext.Services.GetRequiredService<OperationService>();
        }
    }
}
