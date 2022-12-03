using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptographyApp.Models
{
    internal class OOperationModel
    {
        public string InPath { get; }

        public string OutPath { get; }

        private async Task TransformStreams(Stream inputStream, Stream outputStream, ICryptoTransform transform)
        {
            var readBuffer = new byte[transform.InputBlockSize];
            var transformBuffer = new byte[transform.OutputBlockSize];

            while (inputStream.Position != inputStream.Length)
            {
                var readCount = await inputStream.ReadAsync(readBuffer, 0, transform.InputBlockSize);
                if (readCount != transform.InputBlockSize)
                {
                    var finalBlock = transform.TransformFinalBlock(readBuffer, 0, readCount);
                    await outputStream.WriteAsync(finalBlock, 0, finalBlock.Length);
                }
                else
                {
                    transform.TransformBlock(readBuffer, 0, readBuffer.Length, transformBuffer, 0);
                    await outputStream.WriteAsync(transformBuffer, 0, transformBuffer.Length);
                }
            }
        }

        public async Task Run()
        {
            using (var inFile = new FileStream(InPath, FileMode.Open, FileAccess.Read))
            {
                using (var outFile = new FileStream(OutPath, FileMode.Create, FileAccess.Write))
                {
                    var aes = Aes.Create();
                    var transform = aes.CreateEncryptor();

                    
                }
            }
        }
    }
}
