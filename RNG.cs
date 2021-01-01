using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class RNG
    {
        private static readonly Random _generator = new Random();
        public static int Generator(int minVal, int maxVal)
        {
            return _generator.Next(minVal, maxVal + 1);
        }


    }
}
