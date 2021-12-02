using System;
using System.Linq;

namespace Core.Utilities.helper
{
    public class RandomGenerator
    {
        public static string GetRandomString(int length)
        {
            var r = new Random();
            return new string(Enumerable.Range(0, length).Select(n => (char) r.Next(32, 127)).ToArray());
        }
    }
}