using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace uploader
{
    internal static class Utils
    {
        public static string GetMD5(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    var hashBytes = md5.ComputeHash(stream);
                    var sb = new StringBuilder();
                    foreach (var t in hashBytes)
                    {
                        sb.Append(t.ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }

        public static string GetSHA256(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                var sha = new SHA256Managed();
                var checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
        }

        public static string GetSHA1(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                var sha = new SHA1Managed();
                var checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
        }

        private const long OneKB = 1024;
        private const long OneMB = OneKB * OneKB;
        private const long OneGB = OneMB * OneKB;
        private const long OneTB = OneGB * OneKB;
        public static string BytesToHumanReadable(ulong bytes) => bytes switch
        {
            (< OneKB) => $"{bytes} B",
            (>= OneKB) and (< OneMB) => $"{(decimal)bytes / OneKB:F2} KB",
            (>= OneMB) and (< OneGB) => $"{(decimal)bytes / OneMB:F2} MB",
            (>= OneGB) and (< OneTB) => $"{(decimal)bytes / OneGB:F2} GB",
            (>= OneTB) => $"{(decimal)bytes / OneTB:F2} TB"
            //...
        };

        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        public static string Encode(this string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decode(this string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }

        public static string OneLine(this string text)
        {
            return text.Replace("\r", "").Replace("\n", " ");
        }

    }

    [Serializable]
    public class MyException : Exception
    {
        public MyException()
        { }

        public MyException(string message)
            : base(message)
        { }

        public MyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
