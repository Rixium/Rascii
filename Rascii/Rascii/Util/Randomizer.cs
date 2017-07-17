using System;

namespace Rascii.Util
{
    class Randomizer
    {

        public static int RandomInt(int min, int max)
        {
            int i;
            Random r = new Random(Guid.NewGuid().GetHashCode());

            return r.Next(min, max);
        }

    }
}
