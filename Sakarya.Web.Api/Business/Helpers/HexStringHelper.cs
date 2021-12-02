using System;
using System.Linq;

namespace Business.Helpers
{
    public static class HexStringHelper
    {
        private static readonly Random _RND = new Random();

        public static string GenerateHexString(int digits)
        {
            return string.Concat(Enumerable.Range(0, digits).Select(_ => _RND.Next(16).ToString("X")));
        }
    }
}