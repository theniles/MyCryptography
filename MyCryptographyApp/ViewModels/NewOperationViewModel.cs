using MyCryptographyApp.Helper;
using MyCryptographyApp.Models;
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

        public ICommand BrowseInputPath { get; }

        public ICommand BrowseOutputPath { get; }

        private string m_KeyString;
        public string KeyString { get { return m_KeyString; } set { SetProperty(ref m_KeyString, value); } }

        private SymmetricCryptographyOperationType m_CryptographyOperationType;
        public SymmetricCryptographyOperationType CryptographyOperationType { get { return m_CryptographyOperationType; } set { 
                if(CryptographyOperationType != value)
                {
                    var prefix = StringHelper.GetSymmetricCryptographyOperationTypePrefix(CryptographyOperationType);
                    if (OutputFile != null && OutputFile.StartsWith(prefix))
                    {
                        OutputFile = 
                            StringHelper.GetSymmetricCryptographyOperationTypePrefix(
                            CryptographyOperationTypeHelper.ReverseType(CryptographyOperationType)) +
                            OutputFile.Substring(prefix.Length, OutputFile.Length - prefix.Length);
                    }
                }
                SetProperty(ref m_CryptographyOperationType, value); } }

        private string m_InputPath;
        public string InputFullPath { get { return m_InputPath; } set { SetProperty(ref m_InputPath, value); } }

        private string m_OutputPath;
        public string OutputFolder { get { return m_OutputPath; } set { SetProperty(ref m_OutputPath, value); } }


        private string m_OutputName;
        public string OutputFile { get { return m_OutputName; } set { SetProperty(ref m_OutputName, value); } }

        private OperationService OperationService { get; }

        public NewOperationViewModel()
        {
            ExecuteOperation = new Command(async () =>
            {
                if (string.IsNullOrWhiteSpace(KeyString) || string.IsNullOrWhiteSpace(OutputFolder) || string.IsNullOrWhiteSpace(OutputFile) || string.IsNullOrWhiteSpace(InputFullPath))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please enter all details", "OK");
                    return;
                }

                var outputPath = Path.Combine(OutputFolder, OutputFile);

                var operationName = CryptographyOperationType switch
                {
                    SymmetricCryptographyOperationType.Encryption => $"ENCRYPTION {Path.GetFileName(InputFullPath)} to {OutputFile}",
                    SymmetricCryptographyOperationType.Decryption => $"DECRYPTION {InputFullPath} to {outputPath}",
                    _ => throw new NotImplementedException()
                };
                var op = new SymmetricCryptographyOperationViewModel(operationName, Aes.Create, SHA256.Create, CryptographyOperationType, InputFullPath, outputPath, Encoding.Unicode.GetBytes(KeyString));
                OperationService.AddOperation(op);
            });

            BrowseInputPath = new Command(async () =>
            {
                try
                {
                    var file = await FilePicker.PickAsync();
                    if (file != null)
                    {
                        InputFullPath = file.FullPath;
                        OutputFolder = Path.GetDirectoryName(file.FullPath) ?? "";
                        OutputFile = $"{StringHelper.GetSymmetricCryptographyOperationTypePrefix(CryptographyOperationType)} {file.FileName}";
                    }
                }
                catch (TaskCanceledException)
                {

                }
            });

            OperationService = App.Current.Handler.MauiContext.Services.GetRequiredService<OperationService>();
        }
    }
}
