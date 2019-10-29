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
    }
}