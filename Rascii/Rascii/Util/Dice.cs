using System;

namespace Rascii.Util
{
    class Dice
    {

        public static int Roll(int count, int sides)
        {
            int number = 0;
            Random r = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < count; i++)
            {
                number += r.Next(1, sides);
            }

            return number;
        }

    }
}
