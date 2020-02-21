using System;

namespace Encryption.Helpers
{
    public static class ConversionHelper
    {
        public static string BytesToBase64String(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] Base64StringBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}
