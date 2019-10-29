using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone
{
    public static class Randomness
    {
        private static Random random = new Random();

        public static int RandomInt(int minimum, int maximum)
        {
            return random.Next(minimum, maximum);
        }
        public static string RandomString(int length = 32)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(length).ToArray());
        }
    }
}