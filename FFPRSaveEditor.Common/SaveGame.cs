using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace FFPRSaveEditor.Common
{
    public class SaveGame
    {
        static readonly string Password = "TKX73OHHK1qMonoICbpVT0hIDGe7SkW0";
        static readonly string Saltword = "71Ba2p0ULBGaE6oJ7TjCqwsls1jBKmRL";

        public static string Decrypt(string jsonData)
        {
            byte[] decodedJSONData = Convert.FromBase64String(jsonData);

            byte[] saltwordBytes = Encoding.UTF8.GetBytes(Saltword);

            using var generator = new Rfc2898DeriveBytes(Password, saltwordBytes, 10);

            var blockCipher = new CbcBlockCipher(new RijndaelEngine(256));
            var cipher = new PaddedBufferedBlockCipher(blockCipher, new ZeroBytePadding());
            var parameters = new ParametersWithIV(new KeyParameter(generator.GetBytes(32)), generator.GetBytes(32));

            byte[] decryptedJSONData = new byte[cipher.GetOutputSize(decodedJSONData.Length)];

            try
            {
                cipher.Init(false, parameters);
                int bytesProcessed = cipher.ProcessBytes(decodedJSONData, decryptedJSONData, 0);
                cipher.DoFinal(decryptedJSONData, bytesProcessed);
            }
            catch (Exception)
            {
                throw new Exception("Failed to decrypt stream!");
            }

            using var ds = new DeflateStream(new MemoryStream(decryptedJSONData), CompressionMode.Decompress);
            using var ms = new MemoryStream();

            try
            {
                ds.CopyTo(ms);
            }
            catch (Exception)
            {
                throw new Exception("Failed to decompress stream!");
            }

            byte[] decompressedJSONData = ms.ToArray();

            return Encoding.UTF8.GetString(decompressedJSONData);
        }

        public static string Encrypt(string jsonData)
        {
            using var msJSONData = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));
            using var msCompressedJSONData = new MemoryStream();
            using var ds = new DeflateStream(msCompressedJSONData, CompressionMode.Compress);

            try
            {
                msJSONData.CopyTo(ds);
                ds.Close();
            }
            catch (Exception)
            {
                throw new Exception("Failed to compress stream!");
            }

            byte[] compressedJSONData = msCompressedJSONData.ToArray();

            byte[] saltwordBytes = Encoding.UTF8.GetBytes(Saltword);

            using var generator = new Rfc2898DeriveBytes(Password, saltwordBytes, 10);

            var blockCipher = new CbcBlockCipher(new RijndaelEngine(256));
            var cipher = new PaddedBufferedBlockCipher(blockCipher, new ZeroBytePadding());
            var parameters = new ParametersWithIV(new KeyParameter(generator.GetBytes(32)), generator.GetBytes(32));

            byte[] encryptedJSONData = new byte[cipher.GetOutputSize(compressedJSONData.Length)];

            try
            {
                cipher.Init(true, parameters);
                int bytesProcessed = cipher.ProcessBytes(compressedJSONData, encryptedJSONData, 0);
                cipher.DoFinal(encryptedJSONData, bytesProcessed);
            }
            catch (Exception)
            {
                throw new Exception("Failed to encrypt stream!");
            }

            return Convert.ToBase64String(encryptedJSONData);
        }
    }
}
