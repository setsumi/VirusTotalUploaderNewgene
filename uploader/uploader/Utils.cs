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
